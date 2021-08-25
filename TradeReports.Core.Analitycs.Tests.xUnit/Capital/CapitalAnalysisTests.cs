using Xunit;
using TradeReports.Core.Analitycs.Capital;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeReports.Core.Models;

namespace TradeReports.Core.Analitycs.Capital.Tests
{
    public class CapitalAnalysisTests
    {
        [Fact()]
        public void MovingAverageTest_ItemsLessThanPeriod()
        {
            // Arrange
            List<Operation> operations = new();

            operations.Add(new Operation { CloseDate = DateTime.ParseExact("08/18/2021", "MM/dd/yyyy", null), CapitalDT = 300});
            operations.Add(new Operation { CloseDate = DateTime.ParseExact("08/19/2021", "MM/dd/yyyy", null), CapitalDT = 200 });
            operations.Add(new Operation { CloseDate = DateTime.ParseExact("08/20/2021", "MM/dd/yyyy", null), CapitalDT = 500 });
            operations.Add(new Operation { CloseDate = DateTime.ParseExact("08/21/2021", "MM/dd/yyyy", null), CapitalDT = 100 });
            operations.Add(new Operation { CloseDate = DateTime.ParseExact("08/22/2021", "MM/dd/yyyy", null), CapitalDT = 300 });
            operations.Add(new Operation { CloseDate = DateTime.ParseExact("08/23/2021", "MM/dd/yyyy", null), CapitalDT = 200 });

            CapitalAnalysis capitalAnalysis = new(operations);

            // Act
            Dictionary<DateTime, decimal> movingAverage = capitalAnalysis.MovingAverage(14);

            // Assert
            Assert.Equal(252.38m, Math.Round(movingAverage.Last().Value, 2));
        }

        [Fact]
        public void MovingAverageTest_EmptyOperations()
        {
            // Arrange
            List<Operation> operations = new();

            CapitalAnalysis capitalAnalysis = new(operations);

            // Act
            Dictionary<DateTime, decimal> movingAverage = capitalAnalysis.MovingAverage(14);

            // Assert
            Assert.Empty(movingAverage);
        }

        [Fact]
        public void MovingAverageTest_ElementsMoreThanPeriod()
        {
            // Arrange
            List<Operation> operations = new();

            operations.Add(new Operation { CloseDate = DateTime.ParseExact("08/18/2021", "MM/dd/yyyy", null), CapitalDT = 300});
            operations.Add(new Operation { CloseDate = DateTime.ParseExact("08/19/2021", "MM/dd/yyyy", null), CapitalDT = 200 });
            operations.Add(new Operation { CloseDate = DateTime.ParseExact("08/20/2021", "MM/dd/yyyy", null), CapitalDT = 500 });
            operations.Add(new Operation { CloseDate = DateTime.ParseExact("08/21/2021", "MM/dd/yyyy", null), CapitalDT = 100 });
            operations.Add(new Operation { CloseDate = DateTime.ParseExact("08/22/2021", "MM/dd/yyyy", null), CapitalDT = 300 });
            operations.Add(new Operation { CloseDate = DateTime.ParseExact("08/23/2021", "MM/dd/yyyy", null), CapitalDT = 200 });

            CapitalAnalysis capitalAnalysis = new(operations);

            // Act
            Dictionary<DateTime, decimal> movingAverage = capitalAnalysis.MovingAverage(3);

            // Assert
            Assert.Equal(216.67m, Math.Round(movingAverage.Last().Value, 2));
        }
    }
}