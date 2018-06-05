﻿using System;

namespace Simplic.DataStack
{
    /// <summary>
    /// Represents a report of a stack
    /// </summary>
    public class StackReport
    {
        #region [Fields]

        private Guid id;
        private Guid? reportId;
        private Guid reportTypeId;
        private Guid actionTypeId;
        private Guid stackId;
        private Guid iconId;
        private string text;

        #endregion

        /// <summary>
        /// Initializes a new stack report
        /// </summary>
        public StackReport()
        {
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// Initializes a new stack report
        /// </summary>
        public StackReport(Guid stackId)
        {
            Id = Guid.NewGuid();
            this.stackId = stackId;
        }

        #region [Public Member]

        /// <summary>
        /// Gets or sets the Identifier as Guid
        /// </summary>
        public Guid Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        /// <summary>
        /// Gets or sets the report id
        /// </summary>
        public Guid? ReportId
        {
            get
            {
                return reportId;
            }

            set
            {
                reportId = value;
            }
        }

        /// <summary>
        /// Gets or sets the report type id
        /// </summary>
        public Guid ReportTypeId
        {
            get
            {
                return reportTypeId;
            }

            set
            {
                reportTypeId = value;
            }
        }

        /// <summary>
        /// Gets or sets the action type id
        /// </summary>
        public Guid ActionTypeId
        {
            get
            {
                return actionTypeId;
            }

            set
            {
                actionTypeId = value;
            }
        }

        /// <summary>
        /// Gets or sets the stack id
        /// </summary>
        public Guid StackId
        {
            get
            {
                return stackId;
            }

            set
            {
                stackId = value;
            }
        }

        /// <summary>
        /// Gets or sets the icon id
        /// </summary>
        public Guid IconId
        {
            get
            {
                return iconId;
            }

            set
            {
                iconId = value;
            }
        }

        /// <summary>
        /// Gets or sets the display text
        /// </summary>
        public string Text
        {
            get
            {
                return text;
            }

            set
            {
                text = value;
            }
        }

        #endregion
    }
}