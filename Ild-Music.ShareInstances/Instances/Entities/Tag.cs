using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;


namespace ShareInstances.Instances;
public record struct Tag(Guid Id, ReadOnlyMemory<char> Name);