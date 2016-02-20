using Snifles.Data;
using System;
using System.Diagnostics;

namespace Snifles.Application_Layer
{
    [DebuggerDisplay("QC: {Header.QuestionCount} AC: {Header.AnswerCount}")]
    public sealed class DnsPacket
    {
        public readonly DnsHeader Header;
        public readonly DnsQuestion[] Questions;
        public readonly DnsAnswer[] Answers;
        public readonly DnsAnswer[] Authority;
        public readonly DnsAnswer[] Information;

        public DnsPacket(Packet packet)
        {
            Header = (packet.ApplicationHeader as DnsHeader);
            if (Header == null) throw new ArgumentException("Non-Dns packet!");
            NetBinaryReader nbr = new NetBinaryReader(Header.data);

            Questions = new DnsQuestion[Header.QuestionCount];
            for (int i = 0; i < Questions.Length; i++)
            {
                Questions[i] = new DnsQuestion(nbr);
            }

            Answers = new DnsAnswer[Header.AnswerCount];
            for (int i = 0; i < Answers.Length; i++)
            {
                Answers[i] = new DnsAnswer(nbr);
            }

            Authority = new DnsAnswer[Header.AuthorityCount];
            for (int i = 0; i < Authority.Length; i++)
            {
                Authority[i] = new DnsAnswer(nbr);
            }

            Information = new DnsAnswer[Header.AdditionalCount];
            for (int i = 0; i < Information.Length; i++)
            {
                Information[i] = new DnsAnswer(nbr);
            }
        }
    }
}