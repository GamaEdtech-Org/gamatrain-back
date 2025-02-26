namespace GamaEdtech.Common.Identity.DataProtection
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    using GamaEdtech.Common.DataAccess.UnitOfWork;

    using NUlid;

    public class DataProtectionService(IUnitOfWorkProvider unitOfWorkProvider) : IDataProtectionService
    {
        public IEnumerable<XElement>? GetDataProtections()
        {
            var uow = unitOfWorkProvider.CreateUnitOfWork();
            var lst = uow.GetRepository<DataProtectionKey, Ulid>().GetManyQueryable().Select(t => t.Xml).ToList();
            return lst.Select(t => XElement.Parse(t!));
        }

        public void AddDataProtection(DataProtectionKey dataProtectionKey)
        {
            var uow = unitOfWorkProvider.CreateUnitOfWork();
            var repository = uow.GetRepository<DataProtectionKey, Ulid>();
            repository.Add(dataProtectionKey);
            _ = uow.SaveChanges();
        }
    }
}
