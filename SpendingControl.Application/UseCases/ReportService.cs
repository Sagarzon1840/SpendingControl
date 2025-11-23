using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpendingControl.Application.Interfaces;
using SpendingControl.Domain.Interfaces.Repositories;

namespace SpendingControl.Application.UseCases
{
    public class ReportService : IReportService
    {
        private readonly IBudgetRepository _budgetRepo;
        private readonly ISpendTypeRepository _spendTypeRepo;
        private readonly IExpenseRepository _expenseRepo;

        public ReportService(IBudgetRepository budgetRepo, ISpendTypeRepository spendTypeRepo, IExpenseRepository expenseRepo)
        {
            _budgetRepo = budgetRepo;
            _spendTypeRepo = spendTypeRepo;
            _expenseRepo = expenseRepo;
        }

        public async Task<IEnumerable<BudgetVsExecutionItem>> GetBudgetVsExecutionAsync(Guid userId, DateTime from, DateTime to)
        {
            if (userId == Guid.Empty) throw new ArgumentException("userId is required", nameof(userId));
            if (from > to) throw new ArgumentException("from must be <= to", nameof(from));
            if (from.Year != to.Year || from.Month != to.Month) throw new ArgumentException("Range must be within a single month.");

            var year = from.Year;
            var month = from.Month;

            var budgets = await _budgetRepo.GetByUserAndMonthAsync(userId, year, month);
            var spendTypeIds = budgets.Select(b => b.SpendTypeId).Distinct().ToList();
            var spendTypes = await _spendTypeRepo.GetByUserAsync(userId);
            var spendTypesDict = spendTypes.Where(st => spendTypeIds.Contains(st.Id)).ToDictionary(st => st.Id, st => st.Name);

            var executedTuples = await _expenseRepo.GetExecutedByTypeAsync(userId, from, to, spendTypeIds);
            var executedDict = executedTuples.ToDictionary(t => t.Item1, t => t.Item2);

            return budgets.Select(b => new BudgetVsExecutionItem
            {
                ExpenseTypeName = spendTypesDict.TryGetValue(b.SpendTypeId, out var name) ? name : string.Empty,
                Budget = b.Amount,
                Executed = executedDict.TryGetValue(b.SpendTypeId, out var total) ? total : 0m
            }).ToList();
        }
    }
}
