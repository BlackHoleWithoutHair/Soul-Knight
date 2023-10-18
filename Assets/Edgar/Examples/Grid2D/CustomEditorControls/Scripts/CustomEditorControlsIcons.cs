using System;
using UnityEngine;

namespace Edgar.Unity.Examples.CustomEditorControls
{
    /// <summary>
    /// Helper class that holds Base64-encoded icons.
    /// </summary>
    public static class CustomEditorControlsIcons
    {
        public static readonly string TreasureIcon = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAMBAMAAACZySCyAAAAAXNSR0IB2cksfwAAAAlwSFlzAAALEwAACxMBAJqcGAAAABhQTFRFAAAAJRMakJGercHPv3BNiVpFu9PhUjtAfOPVqwAAAAh0Uk5TAP/////////VylQyAAAAVklEQVR4nB3LwQ2AMAxDUWeDBibACHGGignKBKCqCzABUpX1Cfmnd7ABUS8BUOZMBWQstT5bgiyndzly5NCBnHwth72r3Q42sjNgFtitN5v/F714RekDD4EPdyrA9vsAAAAASUVORK5CYII=";

        public static readonly string ShopIcon = "iVBORw0KGgoAAAANSUhEUgAAAAwAAAAMBAMAAACkW0HUAAAAAXNSR0IB2cksfwAAAAlwSFlzAAALEwAACxMBAJqcGAAAAA9QTFRFAAAAJRMawJNE//HM/9VpujpGagAAAAV0Uk5TAP////8c0CZSAAAAOElEQVR4nGNggAFBQRDJqKQkAKSEjE0UgRxlIxUjASCl7OQEpFRUwJSykwqQYhBycVFEaIBqBwMA5UAFlG0lCyUAAAAASUVORK5CYII=";

        public static readonly string SpawnIcon = "iVBORw0KGgoAAAANSUhEUgAAABIAAAAUCAMAAAC3SZ14AAAAAXNSR0IB2cksfwAAAAlwSFlzAAALEwAACxMBAJqcGAAAAVxQTFRFAAAAFxcXKiQjKiQjKyYjKycjKiYiIyMjAAAAKiQkxUk10kw364Yv7Ysu7o4uOS4jJCQkIiIixUk1MCUkIiIixUk22U447IcvMSojKCgoHBwcGhoaIiIixUk254ouMCojJiYmHBwcISEhJSkpJSopyVA91lM+MiwqJSkoKiopKikpIyMjbMnCctbOcNLL7fDn9vLpMTEwHR0dICAgRHeOQ3SMQ3SLKy4wKSkpIiIibMrCb8/JcNLMcNPM7fDo/fftIiIibcrC8O7mHh4eMTAwbcrDRnqQb9DKSnuR8e7nMDAvRHeNbczH7e7k9vDmMC8vIiIibcvDR3WMJCcpIiIiICAga8fA8PTqLy8vQW+FdNHLJy0tIiIibcvE9fXrLy8uIiIia8fBb8/Hb8/I7vDoLy4uP2uDQG2FJCcoIiIiIiIiQG2GJCcoJCQkPmh/PmmAJCYoIiIiIiIiIiIiNFa8owAAAHR0Uk5TAAvs/////x0B7f///////xzh/vT0/v///xMSCu3///8bCez///////////////////8aCP///////////////+7//xH////////////////v////9xD////////w////D//////////19f7/9w7///fm6PYObYMrAAAA4UlEQVR4nGNgYGBkYmZhZWNj52CAAUZOLm4eXj4+fgG4mKCQsIiomDgfn4QkXJmglLSMrJi4nLyCoiJcoZKyiqqauoamljZcoYyOrh4Q6BsYGhlzwIVMTIHAzNzCUgoqZKVrYm2ja6trZ28IFXJwBAqBoZOhM4aQC0zIFSjk5u7m7uHphRDy9gFaaePr5w8VCgjUdQPZGBQcEhIKFQrTA4PwiFCYUwMiTcHOioqGC8XE6oJBXHwCXChRLykpKVkvJRUhlJaekZmZlZadkwsXyssAsmPyC+AiDIUiIHZRMUQEAFs8MfUBWVOtAAAAAElFTkSuQmCC";

        public static readonly string BossIcon = "iVBORw0KGgoAAAANSUhEUgAAABoAAAAfCAMAAADOWS1PAAAAAXNSR0IB2cksfwAAAAlwSFlzAAALEwAACxMBAJqcGAAAAo5QTFRFAAAAICAgIiIiISEhIiIiIiIiIyMjISEhIiIiIiIiIiIiIiIiHBwcHx8fICAgISEhIiIiIiIiISEhIiIiIiIiIiIiIiIiICAgKCMjPiklPCgmNSMpNyQoPSklOycmNCMoIyMjJCQkISEhSSsn2k440E1CrUhmt0lcjz9XIiIiIiIiIiIiJCQkIyMjJiIkMiMnJiIjMSMn4XJf/fft9tPGzsnCMCMnIiIiISEhIiIiMiMoPyUrki5DVCkuhCdFSisn2mtY3NfO3Lmu22xYtbGqjyxDPSQsiilFYTEqz0c8Uysrvkc1nD8xRComv0c1nT8xWCwsiyxCIiIiIyMjOSMqmi9E0Ug800k7TSYwszVHskQzICAgTSQxoSpNszZGqC9LqC5LIiIijyhIpjBIw0k11Ew3xEk11E03xUk1v0g1IyMjISEhIiIidCc/mz8wo0Y3X0Q+XkM+XkM9pEY3Ozo5IyMjS0pIsq6oU1FPzMjAs6+otLCpIyMjICAgTEpJs6+pJCQkISEhISEhIiIiKikpPDs6KSkpIiIiGhoaKiopVFNQy8a/Ozs6Ojo5IyMjIyMjJyIkW0xPt66nYlFMt6+nuK+otq6pW01Psq+oJCQkQSUrkC1CVyouYjEqtkU0VSoujSxDIiIiIyMjPyQshyhFWiwt0Ug700o7zkc8iStBOiMrkChJnylOtDZG2E05r0QzIiIiIiIiTyQyqzBKxUFA0Uk700o6JiYmISEhqEA1IiIiIiIiIiIiIiIiIiIiPSQsmilMjChHkShJmylMdydAIiIiIiIiJCQkIiIiIyMjdic/IiIiIiIiPiQsdidAISEhQCQsdSc/IiIiIiIiOyMrciY+IiIicyc/IiIiIiIiLyBSJAAAANp0Uk5TAAgeH0yrHVXKNUusCSkgNv/SVNHJLd8o/////////9EkHFb///////8lU/cqV////////////8gu3v///////////////////////////////9BY/////////zj//////1n//////////8cv1f//////////1v///////6ow//85xs5a////zQr//////6k6////////////Mf/////////EO//////////////////MW///////G1z/9uPDPPXg//////anNDKjM/bY2eL2Pf/1pKb//8L/16XxWIEqAAACCUlEQVR4nGNgQAaMTAxYASMzIwsrkGLDlGHn4ORgZGLk4kYW5eHl5WPgFxAUEgZCERQNomICguLsEpJS0jKycvJsDIwKinBdSgJAoKyiqqauoqohrKmlDTGUR4dXV0BP38BQWcXI2ETFyNTATMAcosXCUkDPytrG1s5excHRScXZxdrVzJwRYqKFmJu7h4qnq5e3j4Cvn79BgGdgkBZEH09wSGhYOFBOGgwiIlWiBLRZwVLRAnoxsXEqKp7xIJAQqaISBZXhSRQISUpOSU1LB8EMIMz0z4I4IztHTCAk1zfPN78gvxAIi4pLBLJg3souFQP5q6y8orKsCoirtWsQwVELlgNKCgjU1QsgyzAwNAgggUYGIqR4ePmArkQCTYqwAK4Va2oGC7W0gsi2EoF2BSWIfR0Qic7Wru6WnraSru62XgHtPrCB/UDxCYYTJk6aPGXyVCCcNn0GVIZhptgEw1mz58yeOw8EPebNX2C2EOaORYuXLF22XAUOVqxESK1avSZ2rYrKOpj0+g3QBNCxccKmNZuXLVu2ZTNQ77qt21TWC2zYDpbZIbB4DQxsXrZlzeatO3ft3gOS2ruvdP9qoOiBgyBwCMg6fOTosaMgqeMnGE4C5Q6cOg0EZ84eWnP43FGkIDx/4cDFS1DW4XOXkUP3/JWr12Cs69dQAv5k6Y0TENaxM2duQgUBSA283jCE23MAAAAASUVORK5CYII=";

        public static readonly string LockIcon = "iVBORw0KGgoAAAANSUhEUgAAADAAAAAwCAYAAABXAvmHAAAAAXNSR0IArs4c6QAAAm5JREFUaEPtmDtrVUEUhb+LoKAmiJamMRIJCBYq+AcE7YKaKBL/hUi0CpY+0B8hivgg1tpbqWAl8YGNlhF8IJpCZYUJjIc7Z/acmZtzbzgbbnNmZs9a+zFr7vQYceuNOH5KE9gKnAJmgMPAhAvQJ+AV8MT9VksFriSBM8B1YDIC7gNwCVgqQaIEgS3ANeBiIqAbwGXgT+K6/6aXIHAzAP438BHWynQfoPKqmjK20CYBlc2jCoD3wKKr9Z9ubLvrjavA/sp89Yx6o5HlZEARfVOp+afALPA9gGYceAwc98ZF+CDQqLFzCJwD7leA6OQJgV+fKhI6kfxMnAUeNklBDoF7wHlv03lA3yx2AbjjTbwL6Fuy5RBYBg64HX8Be4D1mo8B2QGsANvcRPmaji3qN55DQKWyMwPAW2DKrf8BjG00gb/ehi+Bo4kAXgBHvDWNgtlokdu0I9BlwMm8pXR1q7wNnGjabJZN3BwdDs/cPeldbJ2lBwT+NbA75qzw+BfgEPC5zq+FgBRS14M27AEgxQ+ahcC3DSibEMCvwK5cAv5xKV8W0jnZStrPAibJYQ7yPvoSDVhHoEDEqy6SMt5lYLNmwFducTQrK9B6CYWU26Ssw0CgTrmjyjoMBOqUO6qsw0BAIPXy0M9GgoDefU4HCOgRbC5ycrXexHqpeO5eKXysauJjgB6y6qx1AgK3F7gFnHRI9WJ3xQBe04eCQI6+dQSyLlc5oe+u04HoJdVkl4EBnEJ1ylog4LUuospt+UNTp6yDJhBVbguBkLIOGrxJuS0EqsoauqiVIqTbrFm5rQRKgSvupyNQPKSJDkc+A/8AIdOLMdizYtUAAAAASUVORK5CYII=";

        /// <summary>
        /// Converts a Base64 string to a Texture2D
        /// </summary>
        /// <returns>Texture2D containing an image from the given Base64 string</returns>
        public static Texture2D Base64ToTexture(string base64, FilterMode filterMode = FilterMode.Bilinear)
        {
            var texture = new Texture2D(1, 1);
            texture.hideFlags = HideFlags.HideAndDontSave;
            texture.filterMode = filterMode;
            texture.LoadImage(Convert.FromBase64String(base64));
            return texture;
        }
    }
}