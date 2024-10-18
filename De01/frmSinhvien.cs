using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using DAL.Entities;

namespace De01
{
    public partial class frmSinhvien : Form
    {
        private readonly StudentService studentService = new StudentService();
        private readonly ClassService classService = new ClassService();
        public frmSinhvien()
        {
            InitializeComponent();
        }

       

        private void frmSinhvien_Load(object sender, EventArgs e)
        {
            try
            {
                setGridViewStyle(dgvSinhVien);
                var Classes = classService.GetAll();
                var Students = studentService.GetAll();
                FillClassComboBox(Classes);
                BindGrid(Students);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FillClassComboBox(List<Lop> classes)
        {
            cmbLop.DataSource = classes;
            cmbLop.DisplayMember = "TenLop";
            cmbLop.ValueMember = "MaLop";
        }

        private void BindGrid(List<Sinhvien> students)
        {
            dgvSinhVien.Rows.Clear();
            foreach (var item in students)
            {
                int index = dgvSinhVien.Rows.Add();
                dgvSinhVien.Rows[index].Cells[0].Value = item.MaSV;
                dgvSinhVien.Rows[index].Cells[1].Value = item.HotenSV;
                
                dgvSinhVien.Rows[index].Cells[2].Value = item.NgaySinh.Value.Date.ToString("dd/MM/yyyy");
                if (item.Lop != null)
                {
                    dgvSinhVien.Rows[index].Cells[3].Value = item.Lop.TenLop;
                }
                
            }
        }

        public void setGridViewStyle(DataGridView dgv)
        {
            dgv.BorderStyle = BorderStyle.None;
            dgv.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.BackgroundColor = Color.White;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        public void LoadData()
        {
            List<Sinhvien> students = studentService.GetAll();
            BindGrid(students);
        }
        private void ResetInput()
        {
            txtMSSV.Clear();
            txtTenSV.Clear();
            cmbLop.SelectedIndex = 0;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMSSV.Text) || string.IsNullOrWhiteSpace(txtTenSV.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                    return;
                }

                if (txtMSSV.Text.Trim().Length != 6)
                {
                    MessageBox.Show("Mã số sinh viên phải có 6 kí tự!");
                    return;
                }

                DateTime selectedDate = dtpNgaySinh.Value;
                if (selectedDate < new DateTime(1900, 1, 1) || selectedDate > new DateTime(2079, 6, 6))
                {
                    MessageBox.Show("Ngày sinh không hợp lệ. Ngày phải nằm trong khoảng từ 01/01/1900 đến 06/06/2079.");
                    return;
                }

                Sinhvien newStudent = new Sinhvien()
                {
                    MaSV = txtMSSV.Text.Trim(),
                    HotenSV = txtTenSV.Text.Trim(),
                    NgaySinh = selectedDate,
                    MaLop = cmbLop.SelectedValue.ToString().Trim(), 
                };

                studentService.InsertUpdate(newStudent);
                MessageBox.Show("Thêm mới dữ liệu thành công!");

                LoadData();
                ResetInput();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                string studentID = txtMSSV.Text.Trim();

                if (string.IsNullOrWhiteSpace(studentID))
                {
                    MessageBox.Show("Vui lòng nhập mã số sinh viên để xoá.");
                    return;
                }

                Sinhvien onGoingDeleteStudent = studentService.FindById(studentID);

                if (onGoingDeleteStudent != null)
                {
                    DialogResult dialogResult = MessageBox.Show("Bạn có chắc muốn xoá sinh viên này không?",
                        "Xác nhận xoá",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (dialogResult == DialogResult.Yes)
                    {
                        studentService.Delete(onGoingDeleteStudent);
                        MessageBox.Show("Xoá sinh viên thành công");

                        LoadData();
                        ResetInput();
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sinh viên");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void dgvSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.RowIndex < dgvSinhVien.Rows.Count && dgvSinhVien.Rows[e.RowIndex].Cells[0].Value != null)
                {
                    DataGridViewRow row = dgvSinhVien.Rows[e.RowIndex];

                    txtMSSV.Text = row.Cells[0].Value?.ToString();
                    txtTenSV.Text = row.Cells[1].Value?.ToString();

                    if (DateTime.TryParse(row.Cells[2].Value?.ToString(), out DateTime dateOfBirth))
                    {
                        dtpNgaySinh.Value = dateOfBirth;
                    }

                    string className = row.Cells[3].Value?.ToString();
                    var selectedClass = classService.FindByName(className);

                    if (selectedClass != null)
                    {
                        cmbLop.SelectedValue = selectedClass.MaLop;
                    }
                    else
                    {
                        cmbLop.SelectedIndex = -1;
                    }
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                MessageBox.Show("Ngoài phạm vi hợp lệ: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMSSV.Text) || string.IsNullOrWhiteSpace(txtTenSV.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                    return;
                }

                if (txtMSSV.Text.Trim().Length != 6)
                {
                    MessageBox.Show("Mã số sinh viên phải có 6 kí tự!");
                    return;
                }

                DialogResult dialogResult = MessageBox.Show("Bạn có chắc muốn cập nhật thông tin sinh viên này không?",
                    "Xác nhận cập nhật",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (dialogResult == DialogResult.Yes)
                {
                    string studentID = txtMSSV.Text.Trim();
                    Sinhvien existingStudent = studentService.FindById(studentID);

                    if (existingStudent != null)
                    {
                        existingStudent.HotenSV = txtTenSV.Text.Trim();
                        existingStudent.NgaySinh = dtpNgaySinh.Value;

                        if (cmbLop.SelectedValue != null)
                        {
                            existingStudent.MaLop = cmbLop.SelectedValue.ToString().Trim();
                        }
                        else
                        {
                            MessageBox.Show("Vui lòng chọn lớp hợp lệ!");
                            return;
                        }

                        studentService.InsertUpdate(existingStudent);
                        MessageBox.Show("Cập nhật dữ liệu thành công");

                        LoadData();
                        ResetInput();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy sinh viên cần sửa!");
                    }
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            try
            {
                string searchName = txtTimKiem.Text.Trim(); 

                if (string.IsNullOrWhiteSpace(searchName))
                {
                    MessageBox.Show("Vui lòng nhập tên sinh viên để tìm kiếm!");
                    return;
                }

                List<Sinhvien> result = studentService.SearchByName(searchName);

                if (result.Count > 0)
                {
                    BindGrid(result); 
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sinh viên!");
                    BindGrid(studentService.GetAll()); 
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmSinhvien_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc muốn thoát", "Đồng ý thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            BindGrid(studentService.GetAll());
            txtTimKiem.Clear();
        }
    }
}
