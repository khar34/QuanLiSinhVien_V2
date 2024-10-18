using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;

namespace BLL
{
    public class StudentService
    {
        public List<Sinhvien> GetAll()
        {
            StudentContextDB context = new StudentContextDB();
            return context.Sinhviens.ToList();
        }

        public void InsertUpdate(Sinhvien s)
        {
            StudentContextDB context = new StudentContextDB();
            context.Sinhviens.AddOrUpdate(s);
            context.SaveChanges();
        }

        public Sinhvien FindById(String studentID)
        {
            StudentContextDB context = new StudentContextDB();
            return context.Sinhviens.FirstOrDefault(p => p.MaSV == studentID);
        }

        public void Delete(Sinhvien student)
        {
            using (var context = new StudentContextDB())
            {
                var existingStudent = context.Sinhviens.Find(student.MaSV);

                if (existingStudent != null)
                {
                    context.Sinhviens.Remove(existingStudent);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Sinh viên không tồn tại trong cơ sở dữ liệu."); 
                }
            }

        }

        public List<Sinhvien> SearchByName(string studentName)
        {
            StudentContextDB context = new StudentContextDB();
            return context.Sinhviens
                .Where(s => s.HotenSV.Contains(studentName))
                .ToList();

        }
    }
}
