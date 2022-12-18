using Microsoft.EntityFrameworkCore;
using StudentAndEmployeeAppsMVC.Models.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

string dbConnStr = builder.Configuration.GetConnectionString("StudentRecord");
builder.Services.AddDbContext<StudentRecordContext>(options => options.UseSqlServer(dbConnStr));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Employees}/{action=Index}/{id?}");

app.Run();
