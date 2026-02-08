# Interactive Physics Simulation of a Schwarzschild Black Hole
### Интерактивная симуляция физики черной дыры Шварцшильда

![Black Hole Simulation](https://github.com/user-attachments/assets/2c0c056e-c7db-4ecd-b328-d22733e61590)

## 🇬🇧 About the Project
This project is a high-fidelity visual simulation of a static (non-rotating) Black Hole created in **Unity**. It is designed for educational purposes to demonstrate gravitational lensing, event horizon physics, and orbital mechanics.

**Key Features:**
*   **Scientific Accuracy:** Uses the **Paczynski-Wiita potential** ($1/(r-R_s)^2$) instead of standard Newtonian gravity. This allows for the simulation of relativistic effects like the **ISCO** (Innermost Stable Circular Orbit) and the instability of orbits near the horizon.
*   **Gravitational Lensing:** Real-time visual distortion of light (accretion disk) caused by the black hole's gravity, implemented via shaders.
*   **Real-time Data:** Displays live calculations of gravitational acceleration ($g$), Schwarzschild radius ($R_s$), and distance relative to the horizon.
*   **Math:** Solves quadratic equations to detect precise collision with the Event Horizon.

---

## 🇷🇺 О проекте
Этот проект представляет собой высококачественную визуальную симуляцию статичной (невращающейся) чёрной дыры, созданную на **Unity**. Проект разработан для образовательных целей: демонстрации гравитационного линзирования, физики горизонта событий и орбитальной механики.

**Ключевые особенности:**
*   **Физическая достоверность:** Используется **потенциал Пачинского — Вииты** ($1/(r-R_s)^2$) вместо стандартной ньютоновской гравитации. Это позволяет моделировать релятивистские эффекты, такие как **ISCO** (последняя устойчивая орбита) и неизбежное падение при пересечении критической границы.
*   **Гравитационное линзирование:** Визуализация искривления света (аккреционного диска) гравитацией в реальном времени с помощью шейдеров.
*   **Данные в реальном времени:** На экран выводятся живые расчеты ускорения свободного падения ($g$), радиуса Шварцшильда ($R_s$) и относительного расстояния.
*   **Математика:** Программное решение квадратных уравнений для точного определения столкновения с Горизонтом событий.

---

## 🎮 Controls / Управление

Hold **Left Mouse Button (LMB)** to rotate the camera.
Зажмите **Левую кнопку мыши (ЛКМ)** для вращения камеры.

| Action (EN) | Действие (RU) | Input / Клавиша |
|-------------|---------------|-----------------|
| Camera Look | Вращение камеры | Mouse Movement (with LMB) |
| Move Forward/Back | Вперёд / Назад | **W / S** |
| Move Left/Right | Влево / Вправо | **A / D** |
| Move Up/Down | Вверх / Вниз | **Space / Ctrl** |
| Boost Speed | Ускорение | Hold **Shift** |
| Slow Down | Замедление | Hold **Alt** |

### UI Controls / Интерфейс
*   **Speed Buttons (Кнопки скорости):** Change simulation time scale (essential due to high gravitational forces). / Изменяют масштаб времени симуляции (важно из-за огромных скоростей).
*   **Gravity Checkbox (Гравитация):** Enable/Disable gravitational pull. / Включает или отключает гравитационное притяжение.
*   **Reset (Сброс позиции):** Return camera to the starting position. / Возвращает камеру в начальную точку.

---

## 🛠️ Installation / Установка
### 🇬🇧 English
1. Go to the **Releases** section on the right side of the page.
2. Download the latest release archive for your operating system.
3. Extract the files and launch the application.

### 🇷🇺 Русский
1. Перейдите в раздел **Releases** в правой части страницы.
2. Скачайте архив с последней версией для вашей операционной системы.
3. Распакуйте файлы и запустите приложение.
