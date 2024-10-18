using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;

namespace BLL
{
    public class ClassService
    {
        public List<Lop> GetAll()
        {
            StudentContextDB context = new StudentContextDB();
            return context.Lops.ToList();
        }
        public Lop FindByName(string className)
        {

            StudentContextDB context = new StudentContextDB();
            return context.Lops.FirstOrDefault(l => l.TenLop == className);
            
        }

       
    }
}
