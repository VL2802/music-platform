using Microsoft.AspNetCore.Authentication.Cookies;
using SharedLibraryML.Interfaces;
using ML_LogicLayer.Services;
using ML_Tests.MockRepository;
using SharedLibraryML.CollectionSection;
using SharedLibraryML;
using ML_DAL;
using ML_LogicLayer.Validators;
using ML_WebApp.Services;
using Microsoft.AspNetCore.Http;
using ML_WebApp.Mappers.CollectionM;
using ML_WebApp.Mappers.MediaM;
using ML_DAL.CollectionDAL;
using ML_DAL.MediaDAL;
using ML_LogicLayer.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddHttpContextAccessor();  

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/LogIn";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(120);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ArtistOnly", policy => policy.RequireRole("ARTIST"));
    options.AddPolicy("ListenerOnly", policy => policy.RequireRole("LISTENER"));
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(15);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Repositories (with injected connection string)

builder.Services.AddScoped<IUserRepository>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var connString = config.GetConnectionString("DefaultConnection");
    return new UserRepository(connString);
});

builder.Services.AddScoped<IMediaRepository>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var connString = config.GetConnectionString("DefaultConnection");
    return new MediaRepository(connString);
});

builder.Services.AddScoped<ICollectionRepository>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var connString = config.GetConnectionString("DefaultConnection");
    return new CollectionRepository(connString);
});

builder.Services.AddScoped<IPasswordResetTokenRepository>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var connString = config.GetConnectionString("DefaultConnection");
    return new PasswordResetTokenRepository(connString);
});


// Services
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<MediaService>();
builder.Services.AddScoped<CollectionService>();
builder.Services.AddScoped<PasswordResetService>();


// Other dependencies
builder.Services.AddScoped<IFile, FileHelper>();
builder.Services.AddScoped<MediaMapper>();
builder.Services.AddScoped<ICollectionMappingStrategy, ArtistCollectionMappingStrategy>();
builder.Services.AddScoped<ICollectionMappingStrategy, PlaylistMappingStrategy>();
builder.Services.AddScoped<CollectionMapper>();
builder.Services.AddScoped<UserContextService>();
builder.Services.AddScoped<ICollectionMediaRepository, CollectionMediaRepository>();
builder.Services.AddScoped<IMediaDeletionValidator, MediaDeletionValidator>();

// Security
builder.Services.AddScoped<ICurrentUserContext, CurrentUserContext>();
builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
builder.Services.AddScoped<LoginAttemptService>();



var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); 
app.UseAuthorization();

app.MapRazorPages();

app.Run();