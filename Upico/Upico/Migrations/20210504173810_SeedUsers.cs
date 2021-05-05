using Microsoft.EntityFrameworkCore.Migrations;

namespace Upico.Migrations
{
    public partial class SeedUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //All passwords is Admin1
            migrationBuilder.Sql(@"INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'8043dba8-724a-4976-9b95-84e082f094f5', N'User', N'USER', N'18665193-98a8-4716-974b-23292c6997e4')");
            migrationBuilder.Sql(@"INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'f03f5eaa-548a-4401-b370-098cfcf7b25c', N'Admin', N'ADMIN', N'd93a5d0f-fbbf-46c0-be91-210c695e4622')");

            migrationBuilder.Sql(@"
                INSERT [dbo].[AspNetUsers] ([Id], [DisplayName], [Bio], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'06e0a2ad-9b40-45d1-9c50-c276c5c65aac', N'Tam', NULL, N'tam', N'TAM', N'tiger@test.com', N'TIGER@TEST.COM', 0, N'AQAAAAEAACcQAAAAEK/O2gxTlySUf4V8fasz3dzNRHzcQHu9T477pR0BSAGgEINNRpXc0r4snb6SgY3ZUg==', N'IPKOI6XZRO67SNLHHZH3PX4HQMVYFKFY', N'c91c07b9-bf1d-4c86-9c1b-62ff6eb62a4c', NULL, 0, 0, NULL, 1, 0)
                INSERT [dbo].[AspNetUsers] ([Id], [DisplayName], [Bio], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'09c691bd-98ca-4a2e-9704-3e6e7363ac7a', N'Nghia', NULL, N'nghiax', N'NGHIAX', N'nghia@test.com', N'NGHIA@TEST.COM', 0, N'AQAAAAEAACcQAAAAEAoHh8MsdZRycxq+mQi2cadNt8ggnDWGRTvYPK0yvS942kE74dhqcmniLyM+lz3XzA==', N'PKSHWW6NXHDMW6L3HYIWOD2T3TDCZD6D', N'272c11ca-4e68-4123-8906-104f3750cedc', NULL, 0, 0, NULL, 1, 0)
                INSERT [dbo].[AspNetUsers] ([Id], [DisplayName], [Bio], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'3caaa6ca-1464-4918-b0f3-4b73ceb373c1', N'Admin', NULL, N'admin', N'ADMIN', N'admin@gmail.com', N'ADMIN@GMAIL.COM', 0, N'AQAAAAEAACcQAAAAEOLF6oQIy8xexADWZVUT7PqYmP1BmlRoX/ARwrEcb+F0o7xA3vqv/NavjsM9Aa4rfQ==', N'MA6ZUTKNF534GZI4NXPBJI655E6D234K', N'affaba8a-a7cd-450d-914e-9d041466aac3', NULL, 0, 0, NULL, 1, 0)
                INSERT [dbo].[AspNetUsers] ([Id], [DisplayName], [Bio], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'3fc84852-c34d-46ff-a3a3-b1f218747ca7', N'Vinh', NULL, N'vinh', N'VINH', N'vinh@gmail.com', N'VINH@GMAIL.COM', 0, N'AQAAAAEAACcQAAAAEAG8E4VP/SrQVXLOtz0TK++ggKnc+HkxvdQOWyzq0EZsBQd0xl15T+Bto51sGvjsZQ==', N'GV7HOI6NMQ2CZDE2B5L4DEZF2D5O5VLB', N'1ae09d8c-e61f-4adf-9356-725b681f0cac', NULL, 0, 0, NULL, 1, 0)
                INSERT [dbo].[AspNetUsers] ([Id], [DisplayName], [Bio], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'ada92964-9729-4146-8375-974cca1940aa', N'HuyBoDo', NULL, N'huy', N'HUY', N'huy@test.com', N'HUY@TEST.COM', 0, N'AQAAAAEAACcQAAAAEBC3I9YyFGegeDpl01nJSTeAxNtgxqPwX6ukN3kz/TowrKx9hsCJ9vOVda/GvEsm9Q==', N'2Q2MREFZA3ULKIMXTEC34QCJ66WYL6V5', N'a86383e6-cb58-4d95-bf03-55796620aaaf', NULL, 0, 0, NULL, 1, 0)
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
