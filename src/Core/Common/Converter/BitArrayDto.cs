namespace GamaEdtech.Common.Converter
{
    using System;
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;
    using System.IO.Compression;
    using System.Text.Json.Serialization;

    internal sealed class BitArrayDto
    {
        private const int CompressLength = 257;

        public BitArrayDto()
        {
        }

        public BitArrayDto([NotNull] BitArray ba)
        {
            L = ba.Length;
            var value = ba.BitArrayToByteArray();

            if (L >= CompressLength)
            {
                var ros = new ReadOnlySpan<byte>(value);
                var maxLength = BrotliEncoder.GetMaxCompressedLength(value.Length);
                var s = new Span<byte>(new byte[maxLength]);
                if (!BrotliEncoder.TryCompress(ros, s, out var bytesWritten, 11, 24))
                {
                    throw new ArgumentException("Cannot compress!");
                }

                B = s[..bytesWritten].ToArray();
            }
            else
            {
                B = value;
            }
        }

        [JsonPropertyName("b")]
        public byte[]? B { get; set; }

        [JsonPropertyName("l")]
        public int L { get; set; }

        public BitArray AsBitArray()
        {
            byte[] bytes;

            if (L >= CompressLength)
            {
                var ros = new ReadOnlySpan<byte>(B);
                var s = new Span<byte>(new byte[L.ByteArrayLength()]);

                if (!BrotliDecoder.TryDecompress(ros, s, out var bytesWritten))
                {
                    throw new ArgumentException("Unable to decompress.");
                }

                bytes = s[..bytesWritten].ToArray();
            }
            else
            {
                bytes = B!;
            }

            return new BitArray(bytes)
            {
                Length = L,
            };
        }
    }
}
