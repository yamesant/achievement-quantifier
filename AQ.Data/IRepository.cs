using AQ.Models;

namespace AQ.Data;

public interface IRepository
{
    Task<AchievementClass?> Insert(AchievementClass achievementClass);
    Task<AchievementClass?> Update(AchievementClass achievementClass);
    Task<List<AchievementClass>> GetAllAchievementClasses();
    Task<AchievementClass?> GetAchievementClassById(long id);
    Task<AchievementClass?> GetAchievementClassByName(string name);
    Task<(int AchievementClassesDeleted, int AchievementsDeleted)> DeleteAchievementClass(long id);
}