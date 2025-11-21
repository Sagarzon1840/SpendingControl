using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SpendingControl.Domain.Entities
{
    // Simple helper to compute next Code for SpendType for a given user's existing spend types.
    public static class SpendTypeCodeGenerator
    {
        public static int NextCode(IEnumerable<SpendType> existing)
        {
            if (existing == null) return 1;
            var list = existing as IList<SpendType> ?? existing.ToList();

            if (!list.Any()) return 1;
            return list.Max(x => x.Code) + 1;
        }
    }
}