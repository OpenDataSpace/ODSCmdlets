// ODSCmdlets - Cmdlets for Powershell and Pash for Open Data Space Management
// Copyright (C) GRAU DATA 2013-2014
//
// Author(s): Stefan Burnicki <stefan.burnicki@graudata.com>
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using OpenDataSpace.Commands;

namespace Test
{
    public class TestShellInterface
    {
        private Runspace _runspace;

        public TestShellInterface()
        {
            _runspace = RunspaceFactory.CreateRunspace();
            _runspace.Open();
            LoadODSCmdletSnapin();
        }

        public Collection<object> Execute(string command)
        {
            return Execute(new string[] { command });
        }

        public Collection<object> Execute(string[] commands)
        {
            Collection<PSObject> results = null;
            Collection<object> resultObjects = new Collection<object>();
            using (var pipeline = _runspace.CreatePipeline())
            {
                foreach (var command in commands)
                {
                    pipeline.Commands.AddScript(command, false);
                }
                results = pipeline.Invoke();
                if (pipeline.Error.Count > 0)
                {
                    throw new MethodInvocationException(pipeline.Error.ToString());
                }
            }
            if (results == null)
            {
                return resultObjects;
            }
            foreach (var curPSObject in results)
            {
                if (curPSObject == null)
                {
                    resultObjects.Add(null);
                }
                else
                {
                    resultObjects.Add(curPSObject.BaseObject);
                }
            }
            return resultObjects;
        }

        public object GetVariableValue(string variableName)
        {
            return _runspace.SessionStateProxy.GetVariable(variableName);
        }

        private void LoadODSCmdletSnapin()
        {
            bool isWindows = Environment.OSVersion.Platform == PlatformID.Win32NT;
            if (isWindows) //we are likely to run Powershell, not Pash, so let's install the Snapin first
            {
                var snapin = new ODSCmdlets();
                try
                {
                    Execute(String.Format("Add-PSSnapin {0}", snapin.Name));
                }
                catch (MethodInvocationException e)
                {
                    throw new RuntimeException(String.Format(
                        "Failed to load PSSnapin '{0}'. Did you install it?", snapin.Name),
                        e
                    );
                }                                            
            }
            else //Pash can load Snapins directly, no need to install it
            {
                string path = new Uri(typeof(ODSCmdlets).Assembly.CodeBase).LocalPath;
                Execute(String.Format("Add-PSSnapIn '{0}'", path));
            }
        }
    }
}

