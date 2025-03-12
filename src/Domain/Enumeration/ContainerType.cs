namespace GamaEdtech.Domain.Enumeration
{
    using GamaEdtech.Common.Data.Enumeration;
    using GamaEdtech.Common.DataAnnotation;

    public sealed class ContainerType : Enumeration<byte>
    {
        [Display]
        public static readonly ContainerType Default = new(nameof(Default), 0);

        [Display]
        public static readonly ContainerType School = new(nameof(School), 1);

        public ContainerType()
        {
        }

        public ContainerType(string name, byte value) : base(name, value)
        {
        }
    }
}
