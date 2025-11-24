namespace WebApplication6
{
    public class Program
    {
        public static void Main(string[] args)
        {

            // Я ебану тут свой комментарий. Чтобы у тебя был конфликт. Реши его таким образом, чтобы остался именно этот комментарий.
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // Я ебану тут свой комментарий. Чтобы у тебя был конфликт. Реши его таким образом, чтобы остался именно этот комментарий.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapRazorPages()
               .WithStaticAssets();

            app.Run();
        }
    }
}
