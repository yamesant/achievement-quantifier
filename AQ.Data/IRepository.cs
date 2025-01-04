using AQ.Models;

namespace AQ.Data;

public interface IRepository
{
    Task<AchievementClass?> Insert(AchievementClass achievementClass);
    Task<Achievement?> Insert(Achievement achievement);
    Task<AchievementClass?> Update(AchievementClass achievementClass);
    Task<Achievement?> Update(Achievement achievement);
    Task<List<AchievementClass>> GetAllAchievementClasses();
    Task<List<Achievement>> GetAllAchievements();
    Task<AchievementClass?> GetAchievementClassById(long id);
    Task<Achievement?> GetAchievementById(long id);
    Task<AchievementClass?> GetAchievementClassByName(string name);
    Task<List<Achievement>> GetAchievementsByClassName(string name);
    Task<(int AchievementClassesDeleted, int AchievementsDeleted)> DeleteAchievementClass(long id);
    Task<int> DeleteAchievement(long id);
}