using Domain;
using Framework.Contracts.Common;
using Infrastructure.StockMarkets.QueryRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Infrastructure.Test
{
    public class IntegrationTests : IClassFixture<TradeMatchingEngineDbContextFixture>, IAsyncDisposable
    {
        private readonly TradeMatchingEngineContext dbContext;

        public IntegrationTests(TradeMatchingEngineDbContextFixture fixture, ITestOutputHelper output)
        {
            fixture.Output = output;
            dbContext = fixture.DbContext;
            dbContext.Database.OpenConnection();
        }

        public async ValueTask DisposeAsync()
        {
            await dbContext.Database.CloseConnectionAsync();
        }

        [Fact]
        public async Task StockMarketEngine_Should_Add_To_Context()
        {
            //Arrange

            //Act
            var actual = await dbContext.StockMarkets.AddAsync(new StockMarketMatchEngine(SeedData.FinancialInstrumentId, SeedData.FinancialInstrumentStockMarketId));

            //Assert
            Assert.NotNull(actual.Entity);
        }

        [Fact]
        public async Task StockMarketEngineRepository_Should_Find_StockMarket()
        {
            //Arrange
            var sut = new StockMarketQueryRepository(dbContext);

            //Act
            var actual = await sut.Get(g => g.Id == SeedData.FinancialInstrumentStockMarketId);

            //Assert
            Assert.NotNull(actual);
        }
    }
}