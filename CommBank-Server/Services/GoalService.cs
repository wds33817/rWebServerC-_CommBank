using CommBank.Models;
using MongoDB.Driver;

namespace CommBank.Services;

public class GoalsService : IGoalsService
{
    private readonly IMongoCollection<Goal> _goalsCollection;


    public GoalsService(IMongoDatabase mongoDatabase)
    {
        _goalsCollection = mongoDatabase.GetCollection<Goal>("Goals");
    }

    public async Task<List<Goal>> GetAsync() {
        try
        {
            // Ensure only Goal documents are fetched
            var goals = await _goalsCollection.Find(g => g.Name != null && g.TargetAmount != 0).ToListAsync();
            //_logger.LogInformation($"Goals count: {goals.Count}");
            return goals;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            //_logger.LogError($"Error fetching goals: {ex.Message}");
            throw;
        }
    }

    public async Task<List<Goal>?> GetForUserAsync(string id) =>
        await _goalsCollection.Find(x => x.UserId == id).ToListAsync();

    public async Task<Goal?> GetAsync(string id) =>
        await _goalsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Goal newGoal) =>
        await _goalsCollection.InsertOneAsync(newGoal);

    public async Task UpdateAsync(string id, Goal updatedGoal) =>
        await _goalsCollection.ReplaceOneAsync(x => x.Id == id, updatedGoal);

    public async Task RemoveAsync(string id) =>
        await _goalsCollection.DeleteOneAsync(x => x.Id == id);
}