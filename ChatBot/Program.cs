using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ChatBot
{
    public enum EStrategy
    {
        rand,
        upseq,
        downseq
    }

    public class ChatBot
    {
        static void Main(string[] args) {
            // init
            ChatBot bot = null;
            while (true) {
                try {
                    bot = Init();
                    break;
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
            }
            do {
                bot.AnswerDo();
                try {                
                    var fromUser = Console.ReadLine();
                    if (fromUser.IndexOf("calculate:") == 0) {
                        Console.WriteLine("Функціональність в розробці");
                    }
                    if (fromUser.IndexOf("strategy:") == 0) {
                        var toParse  = fromUser.Replace("strategy:", string.Empty).Replace(" ", string.Empty);
                        bot.Strategy = (EStrategy)Enum.Parse(typeof(EStrategy), toParse);
                        Console.WriteLine($">Как советовать, так все чатлане. Использую стратегию: {bot.Strategy} ");
                    }
                }
                catch {
                    Console.WriteLine("У тебя в голове мозги или кю?!");
                }
            }
            while (true);
        }

        public void AnswerDo() {
            if(_strategy == EStrategy.rand) {
                Console.WriteLine(Answers[new Random().Next(0, Answers.Length)]);                
            }
            if (_strategy == EStrategy.upseq) {
                Console.WriteLine(Answers[pointerForSomeStrategy]);
                if (pointerForSomeStrategy < Answers.Length - 1) pointerForSomeStrategy++;
            }
            if (_strategy == EStrategy.downseq) {
                Console.WriteLine(Answers[pointerForSomeStrategy]);
                if (pointerForSomeStrategy > 0) pointerForSomeStrategy--;
            }            
        }

        private EStrategy _strategy { get; set; }        
        private int pointerForSomeStrategy { get; set; }

        public string[] Answers {  get; private set; }
        public EStrategy Strategy {
            get { return _strategy; }
            set {
                if(value == EStrategy.downseq) {
                    pointerForSomeStrategy = Answers.Length - 1;
                }
                if (value == EStrategy.upseq) {
                    pointerForSomeStrategy = 0;
                }
                _strategy = value;
            }
        }

        public ChatBot(EStrategy strategy, string[] answers) {
            Strategy = strategy;
            Answers = answers;
        }        

        //check talk_to_me -r rand -f ../answers.txt 
        static ChatBot Init() {
            List<string> init = null;
            do {
                init = Console.ReadLine().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            while (init.Count <5 && init.FirstOrDefault() == "talk_to_me" && init.Contains("-r") && init.Contains("-f"));
            var _stetegy = (EStrategy)Enum.Parse(typeof(EStrategy),init[init.IndexOf("-r") +1]);
            var _answers = File.ReadAllLines(init[init.IndexOf("-f") +1]);
            return new ChatBot(_stetegy, _answers);
        }

    }
}