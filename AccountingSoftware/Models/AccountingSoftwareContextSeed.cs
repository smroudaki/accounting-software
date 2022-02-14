using AccountingSoftware.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingSoftware.Models
{
    public static class AccountingSoftwareContextSeed
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>().HasData(
                    new ApplicationUser
                    {
                        Id = "a0c060fb-eb1f-4132-a14f-afc2ecccd728",
                        UserName = "1",
                        NormalizedUserName = "1",
                        Email = null,
                        NormalizedEmail = null,
                        EmailConfirmed = false,
                        PasswordHash = "AQAAAAEAACcQAAAAEGduCXP6wkO8iRG5duhbeZ1thUWVoNsyiCZoNXuOc7HpfUDfAtB0BxVUMWColWDdxw==",
                        SecurityStamp = "52NWE56246TSEHNP6BTSLIYRCO24AXPH",
                        ConcurrencyStamp = "7201a16a-333c-474d-950b-5f8fb0ac92e2",
                        PhoneNumber = null,
                        PhoneNumberConfirmed = false,
                        TwoFactorEnabled = false,
                        LockoutEnd = null,
                        LockoutEnabled = true,
                        AccessFailedCount = 0,
                        FirstName = "علی",
                        LastName = "تهرانچی"
                    }
                );

            modelBuilder.Entity<CodeGroup>().HasData(
                    new CodeGroup
                    {
                        CodeGroupGuid = Guid.Parse("b7a903f3-2c65-49ef-997b-03966578a4c0"),
                        Key = "TransactionState"
                    },
                    new CodeGroup
                    {
                        CodeGroupGuid = Guid.Parse("8d9375da-33d4-41f0-b589-d91c96af8eb5"),
                        Key = "TransactionType"
                    }
                );

            modelBuilder.Entity<Code>().HasData(
                    new Code
                    {
                        CodeGuid = Guid.Parse("d1605abf-8a46-4f2e-8e22-194298b9fd33"),
                        CodeGroupGuid = Guid.Parse("8d9375da-33d4-41f0-b589-d91c96af8eb5"),
                        Value = "Creditor",
                        DisplayValue = "دریافتی"
                    },
                    new Code
                    {
                        CodeGuid = Guid.Parse("749d242b-55f7-4361-bf1b-42c139411c49"),
                        CodeGroupGuid = Guid.Parse("8d9375da-33d4-41f0-b589-d91c96af8eb5"),
                        Value = "Debtor",
                        DisplayValue = "پرداختی"
                    },
                    new Code
                    {
                        CodeGuid = Guid.Parse("b508bd08-5534-4d26-9ed4-c36575c8d90a"),
                        CodeGroupGuid = Guid.Parse("b7a903f3-2c65-49ef-997b-03966578a4c0"),
                        Value = "Passed",
                        DisplayValue = "وصول شده"
                    },
                    new Code
                    {
                        CodeGuid = Guid.Parse("3d905312-ae57-498c-a733-f5cbaf114940"),
                        CodeGroupGuid = Guid.Parse("b7a903f3-2c65-49ef-997b-03966578a4c0"),
                        Value = "NotPassed",
                        DisplayValue = "وصول نشده"
                    },
                    new Code
                    {
                        CodeGuid = Guid.Parse("fe92b8f8-f206-4273-8ca8-f1ecf70a8197"),
                        CodeGroupGuid = Guid.Parse("b7a903f3-2c65-49ef-997b-03966578a4c0"),
                        Value = "Waiting",
                        DisplayValue = "در انتظار وصول"
                    }
                );

            modelBuilder.Entity<Account>().HasData(
                    new Account
                    {
                        AccountGuid = Guid.Parse("a8a1632c-5a7f-4bd7-a9ac-d10a6375ade0"),
                        UserGuid = "a0c060fb-eb1f-4132-a14f-afc2ecccd728",
                        AccountNumber = "1"
                    }
                );
        }
    }
}
