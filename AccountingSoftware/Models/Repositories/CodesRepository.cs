using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingSoftware.Models.Repositories
{
    public class CodesRepository
    {
        private readonly AccountingSoftwareContext context;

        public CodesRepository(AccountingSoftwareContext context)
        {
            this.context = context;
        }

        public IEnumerable<SelectListItem> GetCodesByCodeGroupGuid(Guid guid)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            var codes = context.Code.Where(x => x.CodeGroupGuid == guid).ToList();

            foreach (var code in codes)
            {
                list.Add(new SelectListItem() { Value = code.CodeGuid.ToString(), Text = code.DisplayValue });
            }

            return list.AsEnumerable();
        }
    }
}
