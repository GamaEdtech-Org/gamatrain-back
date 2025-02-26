namespace GamaEdtech.Common.Identity.DataProtection
{
    using System.Collections.Generic;
    using System.Xml.Linq;

    using GamaEdtech.Common.DataAnnotation;

    [Injectable]
    public interface IDataProtectionService
    {
        IEnumerable<XElement>? GetDataProtections();

        void AddDataProtection(DataProtectionKey dataProtectionKey);
    }
}
