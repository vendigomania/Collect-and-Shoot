
namespace YG
{

    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        // Тестовые сохранения для демо сцены
        // Можно удалить этот код, но тогда удалите и демо (папка Example)
        public int money = 1;                       // Можно задать полям значения по умолчанию

        public int lastSavedPPKey = 1;
        public int lastRegisteredCheckPointIndex = 0;
        public string newPlayerName = "Hello!";
        public bool isCheckPointSaved = false;
        public bool[] openLevels = new bool[3];

        public int IterationsBulletsCapacityUpgrade, IterationsMovementSpeedUpgrade, IterationsHealthUpgrade, IterationsShootingSpeedUpgrade, IterationsMagnetUpgrade, IterationsRegenerationUpgrade, IterationsCriticalHitUpgrade, IterationsFreezingHitUpgrade, IterationsBurningHitUpgrade;

        // Ваши сохранения

        // ...

        // Поля (сохранения) можно удалять и создавать новые. При обновлении игры сохранения ломаться не должны


        // Вы можете выполнить какие то действия при загрузке сохранений
        public SavesYG()
        {
            // Допустим, задать значения по умолчанию для отдельных элементов массива

            openLevels[1] = true;
        }
    }
}
