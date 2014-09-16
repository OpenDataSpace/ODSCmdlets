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
using System.Text;

namespace Test
{
    public class TestShellInterface
    {
        private Runspace _runspace;

        public TestShellInterface()
        {
            _runspace = RunspaceFactory.CreateRunspace();
            _runspace.Open();
            LoadODSCmdletBinary();
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
                    var sb = new StringBuilder();
                    foreach (var error in pipeline.Error.NonBlockingRead())
                    {
                        sb.Append(error.ToString() + Environment.NewLine);
                    }
                    throw new RuntimeException(sb.ToString());
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
            object variable = _runspace.SessionStateProxy.GetVariable(variableName);
            if (variable is PSObject)
            {
                variable = ((PSObject)variable).BaseObject;
            }
            return variable;
        }

        private void LoadODSCmdletBinary()
        {
            bool isWindows = Environment.OSVersion.Platform == PlatformID.Win32NT;
            string path = new Uri(typeof(ODSCmdlets).Assembly.CodeBase).LocalPath;
            if (isWindows) //we are likely to run Powershell 2.0 or higher, let's import it as a module
            {
                try
                {
                    Execute(String.Format("Import-Module {0}", path));
                }
                catch (MethodInvocationException e)
                {
                    throw new RuntimeException(String.Format(
                        "Failed to import module '{0}'. Didn't you build it?", path),
                        e
                    );
                }                                            
            }
            else
            {
                Execute(String.Format("Add-PSSnapIn '{0}'", path));
            }
        }
    }
}

