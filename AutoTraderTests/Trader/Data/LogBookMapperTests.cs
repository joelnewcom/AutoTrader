using System;
using AutoTrader.Data;
using AutoTrader.Models;
using AutoTrader.Trader.Advisor;
using AutoTrader.Trader.DataAccess.PoCoToEntityMappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTraderTests.Trader.Data
{
    [TestClass()]
    public class LogBookMapperTests
    {
        LogBookMapper logBookMapper = new LogBookMapper();
        Guid logBookId = Guid.NewGuid();

        [TestMethod()]
        public void MapAdviceEntityToPoCoDecisionsAdded()
        {
            LogBookEntity logBookEntity = new LogBookEntity(logBookId, "assetPairId", DateTime.Now, "reason");
            logBookEntity.Decisions.Add(new DecisionEntity(logBookId, AdviceTypeEntity.linearSlopeAdvice, AdviceEntity.HoldOn, logBookId));
            LogBook logBook = logBookMapper.mapTo(logBookEntity);
            Assert.AreEqual(1, logBook.decisions.Count);
        }

        [TestMethod()]
        public void MapAdviceEntityToPoCo()
        {
            LogBookEntity logBookEntity = new LogBookEntity(logBookId, "assetPairId", DateTime.Now, "reason");
            LogBook logBook = logBookMapper.mapTo(logBookEntity);
            Assert.AreEqual(0, logBook.decisions.Count);
        }

        [TestMethod()]
        public void MapAdviceEntityToPoCoCheckAdviceEnums()
        {
            LogBookEntity logBookEntity = new LogBookEntity(logBookId, "assetPairId", DateTime.Now, "reason");
            logBookEntity.Decisions.Add(new DecisionEntity(logBookId, AdviceTypeEntity.linearSlopeAdvice, AdviceEntity.HoldOn, logBookId));
            logBookEntity.Decisions.Add(new DecisionEntity(logBookId, AdviceTypeEntity.linearSlopeAdvice, AdviceEntity.Buy, logBookId));
            logBookEntity.Decisions.Add(new DecisionEntity(logBookId, AdviceTypeEntity.linearSlopeAdvice, AdviceEntity.Sell, logBookId));
            LogBook logBook = logBookMapper.mapTo(logBookEntity);
            Assert.AreEqual(Advice.HoldOn, logBook.decisions[0].Advice);
            Assert.AreEqual(Advice.Buy, logBook.decisions[1].Advice);
            Assert.AreEqual(Advice.Sell, logBook.decisions[2].Advice);
            Assert.AreEqual(3, Enum.GetNames(typeof(AdviceEntity)).Length);
        }

        [TestMethod()]
        public void MapAdviceEntityToPoCoCheckAdviceTypeEnums()
        {
            LogBookEntity logBookEntity = new LogBookEntity(logBookId, "assetPairId", DateTime.Now, "reason");
            logBookEntity.Decisions.Add(new DecisionEntity(logBookId, AdviceTypeEntity.alreadyOwnerAdvice, AdviceEntity.HoldOn, logBookId));
            logBookEntity.Decisions.Add(new DecisionEntity(logBookId, AdviceTypeEntity.linearSlopeAdvice, AdviceEntity.Buy, logBookId));
            logBookEntity.Decisions.Add(new DecisionEntity(logBookId, AdviceTypeEntity.alwaysWinAdvice, AdviceEntity.Sell, logBookId));
            logBookEntity.Decisions.Add(new DecisionEntity(logBookId, AdviceTypeEntity.buySafetyCatch, AdviceEntity.Sell, logBookId));
            logBookEntity.Decisions.Add(new DecisionEntity(logBookId, AdviceTypeEntity.sellSafetyCatch, AdviceEntity.Sell, logBookId));
            logBookEntity.Decisions.Add(new DecisionEntity(logBookId, AdviceTypeEntity.enoughMoneyAdvice, AdviceEntity.Sell, logBookId));
            LogBook logBook = logBookMapper.mapTo(logBookEntity);
            Assert.AreEqual(AdviceType.alreadyOwnerAdvice, logBook.decisions[0].AdviceType);
            Assert.AreEqual(AdviceType.linearSlopeAdvice, logBook.decisions[1].AdviceType);
            Assert.AreEqual(AdviceType.alwaysWinAdvice, logBook.decisions[2].AdviceType);
            Assert.AreEqual(AdviceType.buySafetyCatch, logBook.decisions[3].AdviceType);
            Assert.AreEqual(AdviceType.sellSafetyCatch, logBook.decisions[4].AdviceType);
            Assert.AreEqual(AdviceType.enoughMoneyAdvice, logBook.decisions[5].AdviceType);
            Assert.AreEqual(6, Enum.GetNames(typeof(AdviceTypeEntity)).Length);
        }
    }
}
