using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using WebApplication1.Model;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonDKController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public DonDKController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("GetAll")]
        [HttpGet]
        public JsonResult Get()
        {
            try
            {
                DataTable dataTable = new DataTable();
                string dataSource = _configuration.GetConnectionString("QLCaAn");
                string query = "select * from DonDK join NhanVien on NhanVien.ID_NhanVien=DonDK.ID_NhanVien";
                SqlDataReader sqlDataReader;
                using (SqlConnection sqlConnection = new SqlConnection(dataSource))
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
                return new JsonResult(dataTable);
            }
            catch(Exception ex)
            {
                return new JsonResult(ex.ToString());
            }
        }
		[Route("Insert")]
		[HttpPost]
		public JsonResult Insert(DonDK donDK)
		{
			try
			{
				DataTable dataTable = new DataTable();
				string dataSource = _configuration.GetConnectionString("QLCaAn");
				string query = "Insert into DonDK(ID_DonDK,LoaiDK,ID_NhanVien,CaAn) values (@ID_DonDK,@LoaiDK,@ID_NhanVien,@CaAn)";
				SqlDataReader sqlDataReader;
				using (SqlConnection sqlConnection = new SqlConnection(dataSource))
				{
					sqlConnection.Open();
					using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
					{
						sqlCommand.Parameters.AddWithValue("@ID_DonDK", donDK.ID_DonDK);
						//sqlCommand.Parameters.AddWithValue("@NgayDK", donDK.NgayDK);
						sqlCommand.Parameters.AddWithValue("@LoaiDK", donDK.LoaiDK);
						sqlCommand.Parameters.AddWithValue("@CaAn", donDK.CaAn);
						sqlCommand.Parameters.AddWithValue("@ID_NhanVien", donDK.ID_NhanVien);
						sqlDataReader = sqlCommand.ExecuteReader();
						dataTable.Load(sqlDataReader);
						sqlDataReader.Close();
						sqlConnection.Close();
					}
				}
				return new JsonResult("Insert Success");
			}
			catch (Exception ex)
			{
				return new JsonResult(ex.ToString());
			}
		}
		[Route("Upadte")]
		[HttpPut]
		public JsonResult Update(DonDK donDK)
		{
			try
			{
				DataTable dataTable = new DataTable();
				string dataSource = _configuration.GetConnectionString("QLCaAn");
				string query = "Update DonDK set   LoaiDK=@LoaiDK, ID_NhanVien=@ID_NhanVien, CaAn =@CaAn where ID_DonDK=@ID_DonDK";
				SqlDataReader sqlDataReader;
				using (SqlConnection sqlConnection = new SqlConnection(dataSource))
				{
					sqlConnection.Open();
					using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
					{
						sqlCommand.Parameters.AddWithValue("@ID_DonDK", donDK.ID_DonDK);
						
						sqlCommand.Parameters.AddWithValue("@LoaiDK", donDK.LoaiDK);
						sqlCommand.Parameters.AddWithValue("@CaAn", donDK.CaAn);
						sqlCommand.Parameters.AddWithValue("@ID_NhanVien", donDK.ID_NhanVien);
						sqlDataReader = sqlCommand.ExecuteReader();
						dataTable.Load(sqlDataReader);
						sqlDataReader.Close();
						sqlConnection.Close();
					}
				}
				return new JsonResult("Update Success");
			}
			catch (Exception ex)
			{
				return new JsonResult(ex.ToString());
			}
		}
		[Route("Delete{id}")]
		[HttpDelete]
		public JsonResult Delete(string id)
		{
			try
			{
				DataTable dataTable = new DataTable();
				string dataSource = _configuration.GetConnectionString("QLCaAn");
				string query = "Delete from DonDK where ID_DonDK=@ID_DonDK";
				SqlDataReader sqlDataReader;
				using (SqlConnection sqlConnection = new SqlConnection(dataSource))
				{
					sqlConnection.Open();

					using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
					{
						sqlCommand.Parameters.AddWithValue("@ID_DonDK", id);
						
						sqlDataReader = sqlCommand.ExecuteReader();
						dataTable.Load(sqlDataReader);
						sqlDataReader.Close();
						sqlConnection.Close();
					}
				}
				return new JsonResult("Delete Success");
			}
			catch (Exception ex)
			{
				return new JsonResult(ex.ToString());
			}
		}
	}
}
