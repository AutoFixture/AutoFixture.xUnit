namespace Ploeh.AutoFixture
{
    using System;
    using System.IO;
    using Kernel;

    /// <summary>
    /// Trace writer that will write out a trace of object requests and created objects
    /// in the <see cref="ISpecimenBuilder" /> pipeline.
    /// </summary>
    public class BuildTraceWriter : ISpecimenBuilder // : RequestTracker
    {
        //private readonly TextWriter writer;
        private readonly TracingBuilder tracer;
        private Action<TextWriter, object, int> writeRequest;
        private Action<TextWriter, object, int> writeSpecimen;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildTraceWriter"/> class.
        /// </summary>
        /// <param name="writer">The output stream for the trace.</param>
        /// <param name="builder">The <see cref="ISpecimenBuilder"/> to decorate.</param>
        public BuildTraceWriter(TextWriter writer, TracingBuilder tracer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }
            if (tracer == null)
            {
                throw new ArgumentNullException("tracer");
            }        

            this.tracer = tracer;
            this.tracer.SpecimenRequested += (object sender, SpecimenTraceEventArgs e) => this.TraceRequestFormatter(writer, e.Request, e.Depth);
            this.tracer.SpecimenCreated += (object sender, SpecimenCreatedEventArgs e) => this.TraceCreatedSpecimenFormatter(writer, e.Specimen, e.Depth);

            this.TraceRequestFormatter = (tw, r, i) => tw.WriteLine(new string(' ', i * 2) + "Requested: " + r);
            this.TraceCreatedSpecimenFormatter = (tw, r, i) => tw.WriteLine(new string(' ', i * 2) + "Created: " + r);
        }

        /// <summary>
        /// Gets or sets the formatter for tracing a request.
        /// </summary>
        /// <value>The request trace formatter.</value>
        public Action<TextWriter, object, int> TraceRequestFormatter
        {
            get { return this.writeRequest; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                this.writeRequest = value;
            }
        }

        /// <summary>
        /// Gets or sets the formatter for tracing a created specimen.
        /// </summary>
        /// <value>The created specimen trace formatter.</value>
        public Action<TextWriter, object, int> TraceCreatedSpecimenFormatter
        {
            get { return this.writeSpecimen; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                this.writeSpecimen = value;
            }
        }

        #region ISpecimenBuilder Members

        public object Create(object request, ISpecimenContainer container)
        {
            return this.tracer.Create(request, container);
        }

        #endregion
    }
}