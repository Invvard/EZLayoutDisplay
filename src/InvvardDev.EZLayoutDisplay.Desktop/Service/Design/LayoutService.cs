﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using InvvardDev.EZLayoutDisplay.Desktop.Model;
using InvvardDev.EZLayoutDisplay.Desktop.Service.Interface;

namespace InvvardDev.EZLayoutDisplay.Desktop.Service.Design
{
    public class LayoutService : ILayoutService
    {
        public async Task<ErgodoxLayout> GetLayoutInfo(string layoutHashId)
        {
            Debug.WriteLine("Layout retrieved.");

            return await new Task<ErgodoxLayout>(() => new ErgodoxLayout());
        }

        /// <inheritdoc />
        public async Task<ErgodoxLayout> GetErgodoxLayout(string layoutHashId)
        {
            Debug.WriteLine("Layout retrieved.");

            return await new Task<ErgodoxLayout>(() => new ErgodoxLayout());
        }

        /// <inheritdoc />
        public EZLayout PrepareEZLayout(ErgodoxLayout ergodoxLayout)
        {
            Debug.WriteLine("Layout prepared");

            return new EZLayout();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<KeyTemplate>> GetLayoutTemplate()
        {
            var layoutTemplate = new Task<IEnumerable<KeyTemplate>>(() => new List<KeyTemplate>());

            return await layoutTemplate;
        }
    }
}