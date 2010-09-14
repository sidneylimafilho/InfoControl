using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;

namespace InfoControl.Management
{
    #region Example usage code
    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        Console.Write("Connecting to the DNS Server...");
    //        DNSServer d = new DNSServer("vex.nullify.net"); //my internal DNS Server, change to yours.
    //        //You will need to be able to get to it using WMI.
    //        Console.WriteLine("Connected to the DNS Server");

    //        Console.Write("Creating a new zone as a test...");
    //        try
    //        {
    //            d.CreateNewZone("testzone.uk.nullify.net.", DNSServer.NewZoneType.Primary);
    //            Console.WriteLine("OK");
    //        }
    //        catch (Exception)
    //        {
    //            Console.WriteLine("Failed to create a new zone, it probably exists.");
    //        }

    //        Console.Write("Creating a DNS record as a test...");
    //        try
    //        {
    //            d.CreateDNSRecord("testzone.uk.nullify.net.", "test1.testzone.uk.nullify.net. IN CNAME xerxes.nullify.net.");
    //            Console.WriteLine("OK");
    //        }
    //        catch (Exception)
    //        {
    //            Console.WriteLine("Failed to create a new resource record, it probably exists");
    //        }

    //        Console.WriteLine("Getting a list of domains:");
    //        foreach (DNSServer.DNSDomain domain in d.GetListOfDomains())
    //        {
    //            Console.WriteLine("\t" + domain.Name + " (" + domain.ZoneType + ")");
    //            //and a list of all the records in the domain:-
    //            foreach (DNSServer.DNSRecord record in d.GetRecordsForDomain(domain.Name))
    //            {
    //                Console.WriteLine("\t\t" + record);
    //                //any domains we are primary for we could go and edit the record now!
    //            }
    //        }

    //        Console.WriteLine("Fetching existing named entry (can be really slow, read the warning):-");
    //        DNSServer.DNSRecord[] records = d.GetExistingDNSRecords("test1.testzone.uk.nullify.net.");
    //        foreach (DNSServer.DNSRecord record in records)
    //        {
    //            Console.WriteLine("\t\t" + record);
    //            record.Target = "shodan.nullify.net.";
    //            record.SaveChanges();
    //        }
    //    }
    //}
    #endregion

    #region Real code

    /// <summary>
    /// A Microsoft DNS Server class that abstracts out calls to WMI for MS DNS Server
    /// </summary>
    /// <remarks>
    /// WMI Documentation: 
    /// http://msdn.microsoft.com/en-us/library/ms682123(VS.85).aspx
    /// System.Management Documentation: 
    /// http://msdn.microsoft.com/en-us/library/system.management.managementobjectcollection%28VS.71%29.aspx
    /// </remarks>
    /// <c>(c) 2008 Simon Soanes, All Rights Reserved.  No warranties express or implied.
    /// DO NOT redistribute this source code publically without a link to the origin and this copyright
    /// notice fully intact, also please send any modifications back to me at simon@nullifynetwork.com
    /// Including in your software is fine although attribution would be nice.</c>
    public class DNSServer
    {

        #region Supporting classes
        /// <summary>
        /// Different types of DNS zone in MS DNS Server
        /// </summary>
        public enum ZoneType
        {
            DSIntegrated,
            Primary,
            Secondary
        }

        /// <summary>
        /// Different types of DNS zone in MS DNS Server
        /// </summary>
        /// <remarks>For creation of new zones the list is different</remarks>
        public enum NewZoneType
        {
            Primary,
            Secondary,
            /// <remarks>Server 2003+ only</remarks>
            Stub,
            /// <remarks>Server 2003+ only</remarks>
            Forwarder
        }

        /// <summary>
        /// A zone in MS DNS Server
        /// </summary>
        public class DNSDomain
        {
            /// <summary>
            /// Create a DNS zone
            /// </summary>
            /// <param name="name">The name of the DNS zone</param>
            /// <param name="wmiObject">The object that represents it in MS DNS server</param>
            /// <param name="server">The DNS Server it is to be managed by</param>
            public DNSDomain(string name, ManagementBaseObject wmiObject, DNSServer server)
            {
                _name = name;
                _wmiObject = wmiObject;
                _server = server;
            }

            private DNSServer _server = null;

            private string _name = "";

            /// <summary>
            /// The name of the DNS zone
            /// </summary>
            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }

            /// <summary>
            /// The zone type
            /// </summary>
            public ZoneType ZoneType
            {
                get
                {
                    //_wmiObject["ZoneType"].ToString()
                    return (ZoneType)Convert.ToInt32(_wmiObject["ZoneType"]);
                }
            }

            /// <summary>
            /// Is this a reverse DNS zone?
            /// </summary>
            public bool ReverseZone
            {
                get
                {
                    if (_wmiObject["Reverse"].ToString() == "1")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            private ManagementBaseObject _wmiObject = null;

            /// <summary>
            /// Get a list of all objects at the base of this zone
            /// </summary>
            /// <returns>A list of <see cref="DNSRecord"/></returns>
            public DNSRecord[] GetAllRecords()
            {
                return _server.GetRecordsForDomain(_name);
            }

            /// <summary>
            /// Create a new DNS host record
            /// </summary>
            /// <param name="record">The record to create</param>
            public void CreateDNSRecord(DNSRecord record)
            {
                _server.CreateDNSRecord(_name, record.ToString());
            }

            public override string ToString()
            {
                return _name;
            }
        }

        /// <summary>
        /// An entry in a zone
        /// </summary>
        public class DNSRecord
        {
            /// <summary>
            /// Create an class wrapping a DNS record
            /// Defaults to 1 hour TTL
            /// </summary>
            /// <param name="domain"></param>
            /// <param name="recordType"></param>
            /// <param name="target"></param>
            public DNSRecord(string domain, DNSRecordType recordType, string target) :
                this(domain, recordType, target, new TimeSpan(1, 0, 0))
            {
            }

            /// <summary>
            /// Create an class wrapping a DNS record
            /// </summary>
            /// <param name="domain"></param>
            /// <param name="recordType"></param>
            /// <param name="target"></param>
            /// <param name="ttl"></param>
            public DNSRecord(string domain, DNSRecordType recordType, string target, TimeSpan ttl)
            {
                _host = domain;
                _ttl = ttl;
                _target = target;
                _recordType = recordType;
            }

            /// <summary>
            /// Create an class wrapping a DNS record
            /// </summary>
            /// <param name="wmiObject"></param>
            public DNSRecord(ManagementObject wmiObject)
            {
                _wmiObject = wmiObject;
                _host = wmiObject["OwnerName"].ToString();
                _target = wmiObject["RecordData"].ToString();
                string[] recordParts = wmiObject["TextRepresentation"].ToString().Split(' ', '\t');
                if (recordParts.Length > 2)
                {
                    //the third offset is the location in the textual version of the data where the record type is.
                    //counting from zero that is location 2 in the array.
                    _recordType = new DNSRecordType(recordParts[2]);
                }
                _ttl = new TimeSpan(0, 0, Convert.ToInt32(wmiObject["TTL"]));
            }

            private ManagementObject _wmiObject = null;

            private string _target = "";

            /// <summary>
            /// The value of the target is what is written to DNS as the value of a record
            /// </summary>
            /// <remarks>For MX records include the priority as a number with a space or tab between it and the actual target</remarks>
            public string Target
            {
                get { return _target; }
                set { _target = value; }
            }

            /// <summary>
            /// Save the changes to the resource record
            /// </summary>
            public void SaveChanges()
            {
                //We can call modify and if we have the method available it will work as the sub-class may have it!!
                //Some types DO NOT implement it or implement it differently

                ManagementBaseObject parameters = _wmiObject.GetMethodParameters("Modify");
                bool supported = false;

                //This is a cludge based on the various types that are implemented by MS as they didn't stick to a simple value

                //To add more, please refer to 
                if (RecordType.TextRepresentation == "A")
                {
                    parameters["IPAddress"] = _target;
                    parameters["TTL"] = _ttl.TotalSeconds;
                    supported = true;
                }

                if (RecordType.TextRepresentation == "AAAA")
                {
                    parameters["IPv6Address"] = _target;
                    parameters["TTL"] = _ttl.TotalSeconds;
                    supported = true;
                }

                if (RecordType.TextRepresentation == "CNAME")
                {
                    parameters["PrimaryName"] = _target;
                    parameters["TTL"] = _ttl.TotalSeconds;
                    supported = true;
                }

                if (RecordType.TextRepresentation == "TXT")
                {
                    parameters["DescriptiveText"] = _target;
                    parameters["TTL"] = _ttl.TotalSeconds;
                    supported = true;
                }

                if (RecordType.TextRepresentation == "MX")
                {
                    string[] components = _target.Trim().Split(' ', '\t');
                    if (components.Length > 1)
                    {
                        parameters["Preference"] = Convert.ToUInt16(components[0]); //the preference is a UINT16 in MS DNS Server
                        parameters["MailExchange"] = components[1]; //the actual host name
                        //NOT SUPPORTED BY MX ACCORDING TO THE DOCUMENTATION!? parameters["TTL"] = _ttl;
                        supported = true;
                    }
                }

                Exception temporaryException = null;
                try
                {
                    //This supports improving this classes implementation of this method and adding 
                    ManagementBaseObject lastDitchParameters = OnSaveChanges(parameters);
                    if (lastDitchParameters != null)
                    {
                        parameters = lastDitchParameters;
                        supported = true;
                    }
                }
                catch (Exception ex) //catch all as we do not know what someone will modify OnSaveChanges() to throw or cause
                {
                    if (!supported) //if we support the data type already then we don't care about exceptions as at worst case
                        throw;
                    else
                        temporaryException = ex;
                }

                if (supported)
                {
                    try
                    {
                        _wmiObject = (ManagementObject)_wmiObject.InvokeMethod("Modify", parameters, null);
                    }
                    catch (Exception ex)
                    {
                        if (temporaryException != null)
                        {
                            throw new ApplicationException("There were two exceptions, the primary failure" +
                                " was an exception that is encapsulated in this message however additionaly " +
                                "a virtual method that was optional to functionality also threw an exception " +
                                "but this was withheld till after the operation failed. Please examine the" +
                                " InnerException property for copy of the virtual methods exception.  The " +
                                "virtual methods exception message was: " + temporaryException.Message + ".  " +
                                "The primary exceptions message (a " + ex.GetType().FullName.ToString() + ") " +
                                "was: " + ex.Message, temporaryException);
                        }
                        else
                        {
                            throw;
                        }
                    }

                    if (temporaryException != null)
                    {
                        throw new ApplicationException("A virtual method that was optional to functionality " +
                            "threw an exception but this was withheld till after the operation completed " +
                            "successfully, please examine the InnerException property for a full copy of this " +
                            "exception.  The message was: " + temporaryException.Message, temporaryException);
                    }
                }
                else
                {
                    throw new NotSupportedException("The data type you attmpted to use (" +
                        RecordType.TextRepresentation + ") was not supported, please implement support for" +
                    "it by overriding the method OnSaveChanges() and returning an array of filled WMI parameters " +
                    "or by updating this implementation.");
                }
            }

            /// <summary>
            /// Method to override to add additional methods to the DNS save changes support
            /// </summary>
            /// <param name="parametersIn">An array of parameters (not yet filled in if it's an 
            /// unknown type, potentially partially filled for known types)</param>
            /// <returns>An array of filled in parameters, or null if the parameters are unknown</returns>
            public virtual ManagementBaseObject OnSaveChanges(ManagementBaseObject parametersIn)
            {
                return null;
            }

            /// <summary>
            /// Delete a record from DNS
            /// </summary>
            public void Delete()
            {
                _wmiObject.Delete();
                //well that was easy...
            }

            private TimeSpan _ttl = new TimeSpan(0, 1, 0);

            /// <summary>
            /// The time that the resolvers should cache this record for
            /// </summary>
            public TimeSpan Ttl
            {
                get { return _ttl; }
                set { _ttl = value; }
            }

            private DNSRecordType _recordType = null;

            /// <summary>
            /// The record type
            /// </summary>
            public DNSRecordType RecordType
            {
                get { return _recordType; }
            }
            private string _host = "";

            /// <summary>
            /// The location in the DNS system for this record
            /// </summary>
            public string DomainHost
            {
                get { return _host; }
                set { _host = value; }
            }

            public override string ToString()
            {
                return _host + " " + _recordType.ToString() + " " + _target;
            }
        }

        /// <summary>
        /// The type of record in MS DNS Server
        /// </summary>
        public class DNSRecordType
        {
            /// <summary>
            /// Create a new DNS record type
            /// </summary>
            /// <param name="textRepresentation">The type to create</param>
            public DNSRecordType(string textRepresentation)
            {
                _textRepresentation = textRepresentation;
            }
            private string _textRepresentation = "";

            /// <summary>
            /// The text representation of the record type
            /// </summary>
            public string TextRepresentation
            {
                get
                {
                    return _textRepresentation.ToUpper();
                }
            }

            private string _recordMode = "IN";

            /// <summary>
            /// The mode of the record, usually IN but could oneday be something else like OUT
            /// </summary>
            public string RecordMode
            {
                get
                {
                    return _recordMode;
                }
                set
                {
                    _recordMode = value;
                }
            }

            public override string ToString()
            {
                return _recordMode + " " + _textRepresentation;
            }

            #region Some Defaults!
            /// <summary>
            /// An alias
            /// </summary>
            public static DNSRecordType CNAME
            {
                get { return new DNSRecordType("CNAME"); }
            }

            /// <summary>
            /// An IPv4 address
            /// </summary>
            public static DNSRecordType A
            {
                get { return new DNSRecordType("A"); }
            }

            /// <summary>
            /// A reverse host address inside yoursubnet.in-addr.arpa
            /// </summary>
            public static DNSRecordType PTR
            {
                get { return new DNSRecordType("PTR"); }
            }

            /// <summary>
            /// An MX record (mail exchange)
            /// </summary>
            public static DNSRecordType MX
            {
                get { return new DNSRecordType("MX"); }
            }

            /// <summary>
            /// An IPv6 host address
            /// </summary>
            public static DNSRecordType AAAA
            {
                get { return new DNSRecordType("AAAA"); }
            }

            /// <summary>
            /// A text record
            /// </summary>
            public static DNSRecordType TXT
            {
                get { return new DNSRecordType("TXT"); }
            }

            /// <summary>
            /// A nameserver record (domain delegation)
            /// </summary>
            public static DNSRecordType NS
            {
                get { return new DNSRecordType("NS"); }
            }

            /// <summary>
            /// An SOA record (start of authority)
            /// </summary>
            public static DNSRecordType SOA
            {
                get { return new DNSRecordType("SOA"); }
            }

            #endregion
        }

        #endregion

        /// <summary>
        /// Create a new DNS Server connection
        /// </summary>
        /// <param name="server">The hostname, IP or FQDN of a DNS server you have access to with the current credentials</param>
        public DNSServer(string server)
        {
            _server = server;
            _scope = new ManagementScope(String.Format(@"\\{0}\Root\MicrosoftDNS", server), new ConnectionOptions());
            _scope.Connect();  //no disconnect method appears to exist so we do not need to manage the 
            //persistence of this connection and tidy up
        }

        /// <summary>
        /// Create a new DNS Server connection
        /// </summary>
        /// <param name="server">The hostname, IP or FQDN of a DNS server you have access to with the current credentials</param>
        /// <param name="username">The username to connect with</param>
        /// <param name="password">The users password</param>
        public DNSServer(string server, string username, string password)
        {
            _server = server;
            ConnectionOptions co = new ConnectionOptions();
            co.Username = username;
            co.Password = password;
            co.Impersonation = ImpersonationLevel.Impersonate;
            _scope = new ManagementScope(String.Format(@"\\{0}\Root\MicrosoftDNS", server), co);
            _scope.Connect();
        }

        private string _server = "";

        /// <summary>
        /// The server this connection applies to
        /// </summary>
        public string Server
        {
            get { return _server; }
        }

        private ManagementScope _scope = null;

        /// <summary>
        /// Return a list of domains managed by this instance of MS DNS Server
        /// </summary>
        /// <returns></returns>
        public DNSDomain[] GetListOfDomains()
        {
            ManagementClass mc = new ManagementClass(_scope, new ManagementPath("MicrosoftDNS_Zone"), null);
            mc.Get();
            ManagementObjectCollection collection = mc.GetInstances();

            List<DNSDomain> domains = new List<DNSDomain>();
            foreach (ManagementObject p in collection)
            {
                domains.Add(new DNSDomain(p["ContainerName"].ToString(), p, this));
            }

            return domains.ToArray();
        }

        /// <summary>
        /// Return a list of records for a domain, note that it may include records
        /// that are stubs to other domains inside the zone and does not automatically
        /// recurse.
        /// </summary>
        /// <param name="domain">The domain to connect to</param>
        /// <returns></returns>
        public DNSRecord[] GetRecordsForDomain(string domain)
        {
            string query = String.Format("SELECT * FROM MicrosoftDNS_ResourceRecord WHERE DomainName='{0}'", domain);
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(_scope, new ObjectQuery(query));

            ManagementObjectCollection collection = searcher.Get();

            List<DNSRecord> records = new List<DNSRecord>();
            foreach (ManagementObject p in collection)
            {
                records.Add(new DNSRecord(p));
            }

            return records.ToArray();
        }

        /// <summary>
        /// Create a new DNS host record
        /// </summary>
        /// <param name="zone"></param>
        /// <param name="bindStyleHostEntry"></param>
        /// <returns></returns>
        public void CreateDNSRecord(string zone, string bindStyleHostEntry)
        {
            try
            {
                ManagementObject mc = new ManagementClass(_scope, new ManagementPath("MicrosoftDNS_ResourceRecord"), null);
                mc.Get();
                ManagementBaseObject parameters = mc.GetMethodParameters("CreateInstanceFromTextRepresentation");
                parameters["ContainerName"] = zone;
                parameters["DnsServerName"] = _server;
                parameters["TextRepresentation"] = bindStyleHostEntry;
                ManagementBaseObject createdEntry = mc.InvokeMethod("CreateInstanceFromTextRepresentation", parameters, null);
                //return createdEntry; (no reason unless you changed your mind and wanted to delete it?!)
            }
            catch (ManagementException) //the details on this exception appear useless.
            {
                throw new ApplicationException("Unable to create the record " + bindStyleHostEntry + ", please check" +
                    " the format and that it does not already exist.");
            }
        }

        /// <summary>
        /// Create a new DNS host record
        /// </summary>
        /// <param name="zone"></param>
        /// <param name="record"></param>
        public void CreateDNSRecord(string zone, DNSRecord record)
        {
            CreateDNSRecord(zone, record.ToString());
        }

        /// <summary>
        /// Fetch DNS records for a particular name
        /// WARNING: This method has performance issues, iterate over the results of getting all the records for a domain instead.
        /// </summary>
        /// <remarks>Returns a collection as one hostname/entry can have multiple records but it can take longer
        /// than getting all the records and scanning them!</remarks>
        /// <param name="hostName">The name to look up</param>
        /// <returns></returns>
        public DNSRecord[] GetExistingDNSRecords(string hostName)
        {
            string query = String.Format("SELECT * FROM MicrosoftDNS_ResourceRecord WHERE OwnerName='{0}'", hostName);
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(_scope, new ObjectQuery(query));

            ManagementObjectCollection collection = searcher.Get();
            List<DNSRecord> records = new List<DNSRecord>();
            foreach (ManagementObject p in collection)
            {
                records.Add(new DNSRecord(p));
            }

            return records.ToArray();
        }

        /// <summary>
        /// Create a new zone in MS DNS Server
        /// </summary>
        /// <param name="zoneName">The zone to create</param>
        /// <param name="zoneType">The type of zone to create</param>
        /// <returns>The domain</returns>
        public DNSDomain CreateNewZone(string zoneName, NewZoneType zoneType)
        {
            try
            {
                ManagementObject mc = new ManagementClass(_scope, new ManagementPath("MicrosoftDNS_Zone"), null);
                mc.Get();
                ManagementBaseObject parameters = mc.GetMethodParameters("CreateZone");

                /*
                [in]            string ZoneName,
                [in]            uint32 ZoneType,
                [in]            boolean DsIntegrated,   (will always be false for us, if you need AD integration you will need to change this.
                [in, optional]  string DataFileName,
                [in, optional]  string IpAddr[],
                [in, optional]  string AdminEmailName,
                */

                parameters["ZoneName"] = zoneName;
                parameters["ZoneType"] = (UInt32)zoneType;
                parameters["DsIntegrated"] = 0; //false
                ManagementBaseObject createdEntry = mc.InvokeMethod("CreateZone", parameters, null);
                DNSDomain d = new DNSDomain(zoneName, createdEntry, this);
                return d;
            }
            catch (ManagementException) //returns generic error when it already exists, I'm guessing this is a generic answer!
            {
                throw new ApplicationException("Unable to create the zone " + zoneName + ", please check " +
                    "the format of the name and that it does not already exist.");
            }
        }

        public override string ToString()
        {
            return _server;
        }
    }
    #endregion
}
