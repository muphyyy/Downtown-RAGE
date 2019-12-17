var chat_cmds = {
    "me": {
        color: "chat-purple",
        type: "Acción"
    },
    "b": {
        color: "chat-grey",
        type: "OOC"
    },
    "do": {
        color: "chat-greenlime",
        type: "Entorno"
    },
    "f": {
        color: "chat-green",
        type: "Facción"
    },
    "gob": {
        color: "chat-blue",
        type: "Gobierno"
    },
    "mp": {
        color: "chat-brown",
        type: "MP"
    },
    "n": {
        color: "chat-yellow",
        type: "Dudas"
    },
    "a": {
        color: "chat-red",
        type: "Staff"
    },
    "o": {
        color: "chat-orange",
        type: "Administración"
    },
};


$(document).ready(function(){
    //$(".do-nicescrol4").niceScroll(".wrap");
    
    //iniciamos el chat
    if(chat.Initialize(document.getElementById("chat_log"))){
        chat.RegisterChatMessage("me", "13:37", "lol");
        chat.RegisterChatMessage("b", "13:37", "lol");
        chat.RegisterChatMessage("do", "13:37", "lol");
        chat.RegisterChatMessage("f", "13:37", "lol");
        chat.RegisterChatMessage("gob", "13:37", "lol");
        chat.RegisterChatMessage("mp", "13:37", "lol");
        chat.RegisterChatMessage("n", "13:37", "lol");
        chat.RegisterChatMessage("a", "13:37", "lol");
        chat.RegisterChatMessage("o", "", "lol");
    }
});

var chat = {
    initialized: false,
    chat_target: null,
    chat_input_target: null,
    chat_input: false,
    chat_log_container: null,
    Initialize: function(target){
        if(target !== "undefined"){

            console.log("TARGET",target)
            this.initialized = true;
            console.log("TARGET",target)
            this.chat_target = target;
            var row = document.createElement("div");
            row.classList.add("row");
            row.classList.add("no-gutters");
            row.classList.add("d-none");
            row.id = "chat-input";

            var div = document.createElement("div");
            div.classList.add();
            div.classList.add("col-12");
            div.classList.add("d-flex");
            div.classList.add("bg-downtown");
            div.classList.add("border-0");
            div.classList.add("rounded-lg");

            var input = document.createElement("input");

            input.setAttribute("type", "text");
            input.setAttribute("placeholder", "Ekscrive el mensagje..");

            input.classList.add("w-100");
            input.classList.add("bg-transparent");
            input.classList.add("border-0");
            input.classList.add("p-2");
            input.classList.add("outline-none");
            input.classList.add("text-white");                  
                                
            var button = document.createElement("button");

            button.setAttribute("type", "text");

            button.classList.add("bg-transparent");
            button.classList.add("border-0");
            button.classList.add("pr-3");
            button.classList.add("pl-3");
            button.classList.add("text-white");
            button.classList.add("rounded-circle");
            button.classList.add("outline-none");

            var i = document.createElement("i");
            i.classList.add("fa");
            i.classList.add("fa-play");

            i.setAttribute("aria-hidden", "true");
                            
            row.appendChild(div);
            div.appendChild(input);
            div.appendChild(button);
            button.appendChild(i);

            this.chat_log_container = document.getElementById("chat-log-container");
            if(this.chat_log_container){
                this.chat_log_container.appendChild(row);
                this.chat_input_target = row;
            }

            row.addEventListener("keyup", function(e){
                var code = (e.keyCode ? e.keyCode : e.which);
                if(code == 13) {
                    var d = new Date();
                    this.RegisterChatMessage(null, d.toLocaleTimeString(), e.target.value);
                    e.target.value = "";
                }
            }.bind(this));
        }else
            this.initialized = false;
        
        return this.initialized;
    },
    CheckCMD: function(command){
        var obj = null;
        if(this.initialized){
            Object.entries(chat_cmds).forEach(([key, value]) => {
                if(key == command){
                    obj  = {
                        color: value.color,
                        type: value.type
                    };
                    return obj;
                }
            });
            return obj;
        }
        return obj;
    },
    RegisterChatMessage: function(cmd = "", time = "", message){
        //player.SendChatMessage("{player.Name} se tiró un pedo");
        //player.SendChatMessage("[ME] {player.Name} se tiró un pedo");
        if(this.initialized){
            //Li
            var li = document.createElement("li");
            li.classList.add("list-group-item");
            li.classList.add("bg-transparent");
            li.classList.add("text-white");

            //SPAN - time
            var span_time = document.createElement("span");
            span_time.classList.add("badge");
            span_time.classList.add("badge-secondary");
            span_time.classList.add("bg-transparent");
            span_time.classList.add("bg-downtown");
            span_time.classList.add("p-1");
            var d = new Date();
            if(time != "")
                span_time.innerText = time;
            else
                span_time.innerText = d.toLocaleTimeString();

            var cmd_info = this.CheckCMD(cmd);

            li.appendChild(span_time);
            var span_label = null;
            if(cmd_info){
                //SPAN - cmd
                span_label = document.createElement("span");
                span_label.classList.add("badge");
                span_label.classList.add("badge-secondary");
                span_label.classList.add("ml-2");
                span_label.classList.add("bg-transparent");
                span_label.classList.add("bg-downtown");
                span_label.classList.add(cmd_info.color);
                
                span_label.innerText = cmd_info.type;
                li.appendChild(span_label);
            }

            //STRONG - message
            var strong = document.createElement("strong");
            strong.classList.add("ml-2");

            strong.innerText = message;

            li.appendChild(strong);
            console.log(this.chat_target)
            this.chat_target.appendChild(li);

            this.chat_target.scrollTop = this.chat_target.scrollHeight;
        }
    },
    EnableChatInput: function(state){
        if(this.initialized && this.chat_input_target){
            if(state == true){
                this.chat_input = true;
                this.chat_input_target.classList.remove("d-none");
            }else{
                this.chat_input = false;
                this.chat_input_target.classList.add("d-none");
            }
        }
    },
    /**
     * Aplica un estilo a un elemento
     *
     * @param   {object}  element  Elemento al que aplicamos el estilo
     * @param   {object}  style    Estilo a aplicar
     * 
     * Ej.
     * {
            type: 'width',
            value: '100%'
        }
     *
     */
    SetStyle: function(element, styles){
        Object.entries(styles).forEach(([key, value]) => {
            element.style[key] = value;  
        });          
    },
};