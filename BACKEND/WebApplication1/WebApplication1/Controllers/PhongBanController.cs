using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using WebApplication1.Model;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhongBanController : ControllerBase
    {
        private IConfiguration _configuration;
        public PhongBanController(IConfiguration configuration)
        {
            _configuration = configuration;
        }



        [HttpGet]
        public JsonResult Get()
        {
            DataTable dataTable = new DataTable();
            string query = "select * from PhongBan";
            string sqlDataSource = _configuration.GetConnectionString("QLCaAn");
            SqlDataReader sqlDataReader;

        //SqlConnection->mở kết nối tới CSDL
        //    ↓
        //SqlCommand->tạo câu lệnh SQL chạy trên kết nối
        //    ↓
        //SqlDataReader->đọc kết quả trả về từ lệnh SELECT



			try
			{
				using (SqlConnection sqlConnection = new SqlConnection(sqlDataSource))
				{
					sqlConnection.Open();
					using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
					{
						sqlDataReader = sqlCommand.ExecuteReader();
						dataTable.Load(sqlDataReader);
						sqlDataReader.Close();
						sqlConnection.Close();
					}
				}
				return new JsonResult(dataTable);
			}
            catch (Exception ex)
            {
                return new JsonResult("Error");
            }
           

        }
        [HttpPost]
        public JsonResult Insert(PhongBan phongBan)
        {
            //string ssqlConnection = _configuration.GetConnectionString("QLCaAn");
            
            DataTable dataTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("QlCaAn");
            SqlDataReader sqlDataReader;
            string query = "Insert into PhongBan values" + "('"+phongBan.ID_Phong+"' , N'"+phongBan.TenPhong+"')"; 


			try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlDataSource))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                    {
                        sqlDataReader = sqlCommand.ExecuteReader();
                        dataTable.Load(sqlDataReader);
                        sqlDataReader.Close();
                        sqlConnection.Close();
                    }
                }
				return new JsonResult("Success");

			}
            catch(Exception ex)
            {
                return new JsonResult("Lỗi ko insert đc");
            }

            
        }
        [HttpDelete]
        public JsonResult Delete(string maPhong)
        {
            DataTable dataTable = new DataTable();
            string query = "Delete from PhongBan where ID_Phong=  " + "'" + maPhong+"'";
            SqlDataReader sqlDataReader;
            string sqlDataSource = _configuration.GetConnectionString("QLCaAn");


            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlDataSource))
                {
                    sqlConnection.Open();
                    using(SqlCommand sqlCommand = new SqlCommand(query,sqlConnection))
                    {
                        sqlDataReader = sqlCommand.ExecuteReader();
                        dataTable.Load(sqlDataReader);
                        sqlDataReader.Close();
                        sqlConnection.Close();
                    }
                }


                return new JsonResult("Delete Success");
            }
            catch(Exception ex)
            {
                return new JsonResult("ko xóa được");
            }
        }
        [HttpPut]
        public JsonResult Update(PhongBan phongBan)
        {
            DataTable dataTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("QLCaAn");
			string query = "UPDATE PhongBan SET TenPhong = @TenPhong WHERE ID_Phong = @ID_Phong";
			SqlDataReader sqlDataReader;


            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlDataSource))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand(query,sqlConnection))
                    {
						sqlCommand.Parameters.AddWithValue("@TenPhong", phongBan.TenPhong);
						sqlCommand.Parameters.AddWithValue("@ID_Phong", phongBan.ID_Phong);
						sqlDataReader = sqlCommand.ExecuteReader();
                        dataTable.Load(sqlDataReader);
                        sqlDataReader.Close();
                        sqlConnection.Close();
						return new JsonResult("Update Success");
					}
                }
                
            }
            catch(Exception ex)
            {
                return new JsonResult("Error");
            }
        }
    }
}
