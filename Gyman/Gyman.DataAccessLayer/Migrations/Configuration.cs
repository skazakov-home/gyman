using System;
using System.Data.Entity.Migrations;
using System.Linq;
using Gyman.BusinessLogicLayer;

namespace Gyman.DataAccessLayer.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<GymanDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(GymanDbContext context)
        {
            context.Subscriptions.AddOrUpdate(
                new Subscription
                {
                    SubscriptionType = SubscriptionType.S,
                    NumberOfDays = 10,
                    Cost = 25M
                },
                new Subscription
                {
                    SubscriptionType = SubscriptionType.M,
                    NumberOfDays = 20,
                    Cost = 40M
                },
                new Subscription
                {
                    SubscriptionType = SubscriptionType.L,
                    NumberOfDays = 30,
                    Cost = 50M
                });

            context.SaveChanges();

            context.Members.AddOrUpdate(
                m => m.Name,
                new Member
                {
                    Name = "Алеша",
                    Surname = "Попович",
                    Email = "alex@mail.com",
                    Phone = "0000001",
                    Age = 33,
                    Weight = 96.5,
                    Height = 196,
                    SubscriptionId = context.Subscriptions
                        .Single(s => s.SubscriptionType == SubscriptionType.L).Id
                },
                new Member
                {
                    Name = "Добрыня",
                    Surname = "Никитич",
                    Email = "dobr@mail.com",
                    Phone = "0000002",
                    Age = 43,
                    Weight = 106,
                    Height = 189
                },
                new Member
                {
                    Name = "Илья",
                    Surname = "Муромец",
                    Email = "ilya@mail.com",
                    Phone = "0000003",
                    Age = 23,
                    Weight = 86,
                    Height = 192
                });

            context.Trainers.AddOrUpdate(
                t => t.Name,
                new Trainer { Name = "Змей", Surname = "Горыныч", Phone = "6666666" },
                new Trainer { Name = "Соловей", Surname = "Разбойник", Phone = "9999999" }
                );

            context.SaveChanges();

            context.Trainings.AddOrUpdate(
                t => t.Start,
                new Training
                {
                    Start = DateTime.Now,
                    End = DateTime.Now.AddHours(1),
                    MemberId = context.Members.Single(m => m.Name == "Алеша").Id,
                    TrainerId = context.Trainers.Single(t => t.Name == "Змей").Id
                },
                new Training
                {
                    Start = DateTime.Now.AddHours(3),
                    End = DateTime.Now.AddHours(4),
                    MemberId = context.Members.Single(m => m.Name == "Илья").Id,
                    TrainerId = context.Trainers.Single(t => t.Name == "Соловей").Id
                });
        }
    }
}
