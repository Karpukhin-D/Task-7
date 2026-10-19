
using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== ПЛАНЕТАРНЫЙ СЕЙСМОГРАФ ===");
        Console.WriteLine("        База на Луне\n       ");

        // Задание 1. Запись сейсмического импульса
        int energyStorageJoules = 8000; //запас энергии
        double baseNoiseLevel = 1.2; //уровень фонового шума
        double richterMagnitude; //магнитуда толчка
        bool moonquakeDetected; //обнаружение лунотрясения

        RecordSeismicImpulse(ref energyStorageJoules, ref baseNoiseLevel,
            out richterMagnitude, out moonquakeDetected);

        Console.WriteLine($"Магнитуда: {richterMagnitude:F2}");
        Console.WriteLine($"Остаток энергии: {energyStorageJoules} Дж");
        Console.WriteLine($"Глубокое лунотрясение: {moonquakeDetected}\n");

        // Задание 2. Фильтрация акустического эха
        double signalAmplitude = 100; 
        FilterAcousticEcho(ref signalAmplitude); //уменьшение вдвое после первой перегрузки
        Console.WriteLine($"Амплитуда после фильтрации: {signalAmplitude:F2}");

        double coreReflectionIndex = 0.8; //индекс отражения сигнала ядром Луны
        double pWaveSpeed, sWaveSpeed;
        FilterAcousticEcho(ref signalAmplitude, coreReflectionIndex, //вторая перегрузка
            out pWaveSpeed, out sWaveSpeed); 

        Console.WriteLine($"Скорость P-волны: {pWaveSpeed:F0} м/с");
        Console.WriteLine($"Скорость S-волны: {sWaveSpeed:F0} м/с\n");

        // Задание 3. Рекурсивное затухание ударной волны
        double remainingEnergy = TraceCrustShockwave(10000, 2.5, 5); //передача начальной энергии, плотности реголита и радиуса распространения
        Console.WriteLine($"Энергия ударной волны после затухания: {remainingEnergy:F2} Дж\n");

        // Задание 4. Суммарное смещение геофонов
        double displacement = CalculateDisplacementVector(0.5, 1.2, -0.3, 2.5, 0.2, -1.1); //допустимая погрешность и значения смещений
        Console.WriteLine($"Суммарное смещение коры: {displacement:F2} мкм\n");

        // Задание 5. Анализ матрицы сейсмодатчиков
        double[,] sensorGrid =
        {
            { 1.2, 2.1, 1.5, 0.8, 1.1 },
            { 2.0, 3.4, 2.2, 1.0, 0.7 },
            { 1.4, 2.5, 5.8, 2.3, 1.2 },
            { 0.9, 1.3, 2.7, 3.1, 1.5 },
            { 0.6, 0.8, 1.2, 1.4, 2.0 }
        };

        AnalyzeSeismicArrayMatrix(sensorGrid);
    }

    // Задание 1. Метод записи сейсмического импульса
    static void RecordSeismicImpulse(ref int energyStorageJoules,
        ref double baseNoiseLevel, out double richterMagnitude,
        out bool moonquakeDetected)
    {
        if (energyStorageJoules <= 0)
        {
            richterMagnitude = 0;
            moonquakeDetected = false;
            return;
        }

        richterMagnitude = energyStorageJoules / 1000.0 - baseNoiseLevel; //условная магнитуда по энергии и уровню шума
        moonquakeDetected = richterMagnitude >= 2.5; 

        energyStorageJoules = energyStorageJoules / 2; //уменьшаем запас энергии вдвое
        baseNoiseLevel = baseNoiseLevel + 0.1;
    }

    // Задание 2. Перегрузка 1
    static void FilterAcousticEcho(ref double signalAmplitude)
    {
        signalAmplitude = signalAmplitude * 0.5;
    }

    // Задание 2. Перегрузка 2
    static void FilterAcousticEcho(ref double signalAmplitude,
        double coreReflectionIndex, out double pWaveSpeed, out double sWaveSpeed)
    {
        signalAmplitude = signalAmplitude * coreReflectionIndex; //учитываем индекс отражения и изменяем амплитуду сигнала
        pWaveSpeed = 6000 * coreReflectionIndex;
        sWaveSpeed = 3500 * coreReflectionIndex;
    }

    // Задание 3. Метод рекурсивного затухания ударной волны
    static double TraceCrustShockwave(double initialJoules,
        double regolithDensity, int radiusKm)
    {
        if (radiusKm <= 0 || initialJoules < 1)
            return 0;

        double remainingEnergy = initialJoules / (1 + regolithDensity); //уменьшение энергии с учетом плотности реголита

        return TraceCrustShockwave(remainingEnergy, regolithDensity, radiusKm - 1); //вызываем этот же метод снова, уменьшая радиус на 1 км
    }

    // Задание 4. Метод расчета суммарного смещения
    static double CalculateDisplacementVector(double faultTolerance,
        params double[] sensorOffsets)
    {
        double total = 0;

        for (int i = 0; i < sensorOffsets.Length; i++) //перебираем все значения массива смещений
        {
            if (Math.Abs(sensorOffsets[i]) >= faultTolerance) //учитываем смещение, если его абсолютное значение не меньше порога
                total = total + sensorOffsets[i];
        }

        return total; //возвращаем рассчитанную сумму
    }

    // Задание 5. Метод анализа матрицы сейсмодатчиков
    static void AnalyzeSeismicArrayMatrix(double[,] sensorGrid)
    {
        int rows = sensorGrid.GetLength(0); //количество строк матрицы
        int cols = sensorGrid.GetLength(1); //количество столбцов матрицы

        double maxAmplitude = sensorGrid[0, 0];
        int maxRow = 0;
        int maxCol = 0;
        bool diagonalsIntact = true; //считаем, что диагонали целы

        Console.WriteLine("Карта вибраций:");

        for (int i = 0; i < rows; i++) //внешний цикл перебирает строки
        {
            for (int j = 0; j < cols; j++) 
            {
                if (sensorGrid[i, j] > maxAmplitude) 
                {
                    maxAmplitude = sensorGrid[i, j];
                    maxRow = i;
                    maxCol = j;
                }

                if (sensorGrid[i, j] <= 0)
                    diagonalsIntact = false;

                if (sensorGrid[i, j] >= 3)
                    Console.ForegroundColor = ConsoleColor.Red;
                else
                    Console.ForegroundColor = ConsoleColor.Green;

                Console.Write($"{sensorGrid[i, j],5:F1}");
                Console.ResetColor();
            }

            Console.WriteLine();
        }

        Console.WriteLine($"Эпицентр: строка {maxRow}, столбец {maxCol}");
        Console.WriteLine($"Максимальная амплитуда: {maxAmplitude:F1}");
        Console.WriteLine($"Диагонали датчиков целы: {diagonalsIntact}");
    }
}
