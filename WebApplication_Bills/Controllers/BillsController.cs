using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using System.Data;
using WebApplication_Bills.Models;
using WebApplication_Bills.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebApplication_Bills.Controllers
{
    [Authorize]
    public class BillsController : Controller
    {
        public IConfiguration Configuration { get; }
        private readonly UserManager<IdentityUser> _userManager;
        public BillsController(IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            Configuration = configuration;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            using(SqlConnection con = new(Configuration["ConnectionStrings:DefaultConnection"]))
            {
                using(SqlCommand cmd = new("spBILL_INFORMATION_BY_ID", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    con.Open();
                    SqlDataAdapter da = new(cmd);
                    DataTable dt = new();
                    da.Fill(dt);
                    da.Dispose();
                    List<Details> bills = new List<Details>();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        bills.Add(new Details
                        {
                            BillID = Convert.ToInt32(dt.Rows[i][0]),
                            BillDescription = (dt.Rows[i][1]).ToString(),
                            BillAmount = Convert.ToInt32(dt.Rows[i][2]),
                            BillUnitValue = (decimal)dt.Rows[i][3],
                            BillSubTotal = (decimal)dt.Rows[i][4],
                            BillPriceTotal = (decimal)dt.Rows[i][5],
                            BillCreatedBy = (dt.Rows[i][6]).ToString(),
                            BillDetailID = Convert.ToInt32(dt.Rows[i][7]),
                            BillDetailProduct = (dt.Rows[i][8]).ToString(),
                            BillDetailProductDescription = (dt.Rows[i][9]).ToString(),
                            BillDetailAmount = Convert.ToInt32(dt.Rows[i][10]),
                            BillDetailUnitValue = (decimal)dt.Rows[i][11],
                            BillDetailSubTotal = (decimal)dt.Rows[i][12],
                        });
                        ViewBag.Bill = bills;
                        con.Close();
                    }

                    return View();

                }
            }
        }

        public IActionResult Create() 
        {
            return View(); 
        }

        // POST: Bills/CreateBill
        [HttpPost]
        public async Task<ActionResult> Create(Details model)
        {
            try
            {
                // Obtén el usuario actual
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    // Obtén el correo electrónico del usuario
                    string userEmail = user.Email;

                    // Llama al método para insertar los detalles de la factura
                    InsertBillDetails(
                        model.BillDescription,
                        model.BillAmount,
                        model.BillAmount,
                        model.BillSubTotal,
                        model.BillPriceTotal,
                        userEmail, // Usa el correo electrónico del usuario autenticado
                        model.BillDetailProduct,
                        model.BillDetailProductDescription,
                        model.BillDetailAmount,
                        model.BillDetailUnitValue,
                        model.BillDetailSubTotal
                    );

                    TempData["Message"] = "Bill created successfully!";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorMessage = "Unable to find user information.";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred: " + ex.Message;
                return View(model);
            }
        }

        private void InsertBillDetails(
            string description,
            decimal amount,
            decimal unitValue,
            decimal subTotal,
            decimal priceTotal,
            string createdBy,
            string detailName,
            string detailDescription,
            decimal detailAmount,
            decimal detailUnitValue,
            decimal detailSubTotal)

        {

            using (SqlConnection con = new(Configuration["ConnectionStrings:DefaultConnection"]))
            {
                using (SqlCommand cmd = new("spInsert_Bill_Details", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    Console.WriteLine(description);

                    // Agregar los parámetros al comando
                    const int ISV = 15;
                    var  subTotalPriceBill = Convert.ToInt32(amount) * (decimal)unitValue;
                    var ISVPrice = subTotalPriceBill * ISV/100;
                    var totalPriceBill = subTotalPriceBill + ISVPrice;

                    

                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@Amount", Convert.ToInt32(amount));
                    cmd.Parameters.AddWithValue("@UnitValue", (decimal)unitValue);
                    cmd.Parameters.AddWithValue("@SubTotal", subTotalPriceBill);
                    cmd.Parameters.AddWithValue("@PriceTotal", totalPriceBill);
                    cmd.Parameters.AddWithValue("@CreatedBy", createdBy);
                    cmd.Parameters.AddWithValue("@DetailName", detailName);
                    cmd.Parameters.AddWithValue("@DetailDescription", detailDescription);
                    cmd.Parameters.AddWithValue("@DetailAmount", (decimal)detailAmount);
                    cmd.Parameters.AddWithValue("@DetailUnitValue", Convert.ToInt32(detailUnitValue));
                    cmd.Parameters.AddWithValue("@DetailSubTotal", (decimal)detailAmount * Convert.ToInt32(detailUnitValue));

                    // Abrir la conexión y ejecutar el comando
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();    
                }
            }
                    
                    
                
            
        }
    }
}
