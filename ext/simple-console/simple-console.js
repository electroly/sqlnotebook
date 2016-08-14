// IE doesn't support addEventListener
function addEventListenerXP(elem, evnt, func) {
    if (elem.attachEvent) {
        elem.attachEvent("on" + evnt, func);
    } else {
        elem.addEventListener(evnt, func, false);
    }
}

// IE doesn't support setSelectionRange
function setSelectionRangeXP(input, selectionStart, selectionEnd) {
    if (input.setSelectionRange) {
        input.focus();
        input.setSelectionRange(selectionStart, selectionEnd);
    } else if (input.createTextRange) {
        var range = input.createTextRange();
        range.collapse(true);
        range.moveEnd('character', selectionEnd);
        range.moveStart('character', selectionStart);
        range.select();
    }
}

function SimpleConsole(options) {

    if (!options.handleCommand) {
        throw new Error("options.handleCommand is required");
    }

    var handle_command = options.handleCommand;
    var placeholder = options.placeholder || "";
    var autofocus = options.autofocus;
    var storage_id = options.storageID || "simple-console";

    var add_chevron = function(to_element) {
        var icon = document.createElement("div");
        icon.className = 'input-chevron';
        icon.innerHTML = "&gt;&nbsp;";
        to_element.insertBefore(icon, to_element.firstChild);
    };

    var console_element = document.createElement("div");
    console_element.className = "simple-console";

    var output = document.createElement("div");
    output.className = "simple-console-output";
    output.id = "simple-console-output";
    output.setAttribute("role", "log");
    output.setAttribute("aria-live", "polite");

    var input_wrapper = document.createElement("div");
    input_wrapper.className = "simple-console-input-wrapper";
    add_chevron(input_wrapper);

    var input = document.createElement("input");
    input.className = "simple-console-input";
    input.setAttribute("autofocus", "autofocus");
    input.setAttribute("placeholder", placeholder);
    input.setAttribute("aria-label", placeholder);

    console_element.appendChild(output);
    console_element.appendChild(input_wrapper);
    input_wrapper.appendChild(input);

    var open_popup_button;

    var add_button = function(action) {
        var button = document.createElement("button");
        input_wrapper.appendChild(button);
        addEventListenerXP(button, "click", action);
        return button;
    };

    var add_popup_button = function(update_popup) {

        var popup = document.createElement("popup-menu");

        var popup = document.createElement("div");
        popup.className = "popup-menu";
        popup.setAttribute("role", "menu");
        popup.setAttribute("aria-hidden", "true");
        popup.id = "popup" + (~~(Math.random() * 0xffffff)).toString(0x10);

        var popup_button = document.createElement("button");
        popup_button.className = "popup-button";
        popup_button.setAttribute("aria-haspopup", "true");
        popup_button.setAttribute("aria-owns", popup.id);
        popup_button.setAttribute("aria-expanded", "false");
        popup_button.setAttribute("aria-label", "Command history");
        popup_button.setAttribute("title", "Command history");
        //add_command_history_icon(popup_button);

        input_wrapper.appendChild(popup_button);
        input_wrapper.appendChild(popup);

        addEventListenerXP(popup_button, "keydown", function(e) {
            if (e.keyCode === 40) { // Down
                e.preventDefault();
                first_item = popup.querySelector("[tabindex='0']");
                first_item.focus();
            }
        });

        addEventListenerXP(popup, "keydown", function(e) {
            if (e.keyCode === 38) { // Up
                first_item = popup.querySelector("[tabindex='0']");
                if (document.activeElement === first_item) {
                    popup_button.focus();
                }
            }
        }, true);

        var open_popup = function() {
            popup_button.setAttribute("aria-expanded", "true");
            popup.setAttribute("aria-hidden", "false");
            open_popup_button = popup_button;
            update_popup(popup);
        };

        var close_popup = function() {
            popup_button.setAttribute("aria-expanded", "false");
            popup.setAttribute("aria-hidden", "true");
            open_popup_button = null;
        };

        var popup_is_open = function() {
            return popup_button.getAttribute("aria-expanded") == "true";
        };

        var toggle_popup = function() {
            if (popup_is_open()) {
                close_popup();
            } else {
                open_popup();
            }
        };

        addEventListenerXP(popup_button, "click", toggle_popup);

        addEventListenerXP(window, "mousedown", function(e) {
            if (popup_is_open()) {
                if (!(
                    e.target.closest(".popup-button") == popup_button ||
                    e.target.closest(".popup-menu") == popup
                )) {
                    close_popup();
                }
            }
        });

        addEventListenerXP(window, "focusin", function(e) {
            if (popup_is_open()) {
                if (!(
                    e.target.closest(".popup-button") == popup_button ||
                    e.target.closest(".popup-menu") == popup
                )) {
                    e.preventDefault();
                    close_popup();
                }
            }
        });

        popup_button.popup = popup;
        popup_button.openPopup = open_popup;
        popup_button.closePopup = close_popup;
        popup_button.togglePopup = toggle_popup;
        popup_button.popupIsOpen = popup_is_open;
        return popup_button;
    };

    var add_popup_menu_button = function(get_items) {

        var popup_button = add_popup_button(function(menu) {
            menu.innerHTML = "";
            var items = get_items();

            for (var i = 0; i < items.length; i++) {
                var item = items[i];
                if (item.type === "divider") {
                    var divider = document.createElement("hr");
                    //divider.classList.add("menu-divider");
                    divider.className += " menu-divider";
                    menu.appendChild(divider);
                } else {
                    var menu_item = document.createElement("div");
                    //menu_item.classList.add("menu-item");
                    menu_item.className += " menu-item";
                    menu_item.setAttribute("tabindex", 0);
                    addEventListenerXP(menu_item, "click", item.action);
                    menu_item.textContent = item.label;
                    menu.appendChild(menu_item);
                }
            }
        });

        var menu = popup_button.popup;

        addEventListenerXP(menu, "click", function(e) {
            var menu_item = e.target.closest(".menu-item");
            if (menu_item) {
                popup_button.closePopup();
            }
        });

        addEventListenerXP(menu, "keydown", function(e) {
            if (e.keyCode === 38) { // Up
                e.preventDefault();
                var prev = document.activeElement.previousElementSibling;
                while (prev && prev.nodeName === "HR") {
                    prev = prev.previousElementSibling;
                }
                if (prev && prev.className.indexOf("menu-item") !== -1) {
                    prev.focus();
                }
            } else if (e.keyCode === 40) { // Down
                e.preventDefault();
                var next = document.activeElement.nextElementSibling;
                while (next && next.nodeName === "HR") {
                    next = next.nextElementSibling;
                }
                if (next && next.className.indexOf("menu-item") !== -1) {
                    next.focus();
                }
            } else if (e.keyCode === 13 || e.keyCode === 32) { // Enter or Space
                e.preventDefault();
                document.activeElement.click();
            }
        });

        return popup_button;
    };

    addEventListenerXP(window, "keydown", function(e){
        if(e.keyCode === 27){ // Escape
            if(open_popup_button){
                e.preventDefault();
                var popup_button = open_popup_button;
                popup_button.closePopup();
                popup_button.focus();
            }else if(e.target.closest(".simple-console") === console_element){
                input.focus();
            }
        }
    });

    var clear = function() {
        output.innerHTML = "";
    };

    var last_entry;
    var get_last_entry = function(){
        return last_entry;
    };

    var log = function(content) {
        var was_scrolled_to_bottom = output.is_scrolled_to_bottom();

        var entry = document.createElement("div");
        entry.className = "entry";
        var entrySpan = document.createElement("span");
        entrySpan.className = "history-input-command";
        entrySpan.innerText = content;
        entry.appendChild(entrySpan);
        output.appendChild(entry);

        setTimeout(function() {
            if (was_scrolled_to_bottom) {
                output.scroll_to_bottom();
            }
        });

        last_entry = entry;
        return entry;
    };

    var logHTML = function(html) {
        log("");
        get_last_entry().innerHTML = html;
    };

    var error = function(content) {
        log(content);
        get_last_entry().className += " error";
    };

    var warn = function(content) {
        log(content);
        get_last_entry().className += " warning";
    };

    var info = function(content) {
        log(content);
        get_last_entry().className += " info";
    };

    output.is_scrolled_to_bottom = function() {
        return output.scrollTop + output.clientHeight >= output.scrollHeight;
    };

    output.scroll_to_bottom = function() {
        window.scrollTo(0, document.body.scrollHeight);
    };

    var command_history = [];
    var command_index = command_history.length;
    var command_history_key = storage_id + " command history";

    var load_command_history = function() {
        try {
            command_history = JSON.parse(localStorage[command_history_key]);
            command_index = command_history.length;
        } catch (e) {}
    };

    var save_command_history = function() {
        try {
            localStorage[command_history_key] = JSON.stringify(command_history);
        } catch (e) {}
    };

    var clear_command_history = function() {
        command_history = [];
        save_command_history();
    };

    load_command_history();

    addEventListenerXP(input, "keydown", function(e) {
        if (e.keyCode === 13) { // Enter

            var command = input.value;
            if (command === "") {
                return;
            }
            input.value = "";

            if (command_history[command_history.length - 1] !== command) {
                command_history.push(command);
            }
            command_index = command_history.length;
            save_command_history();

            var command_entry = log(command);
            command_entry.className += " input";
            add_chevron(command_entry);

            output.scroll_to_bottom();

            handle_command(command);

        } else if (e.keyCode === 38) { // Up
            
            if (--command_index < 0) {
                command_index = -1;
                input.value = "";
            } else {
                input.value = command_history[command_index];
            }
            setSelectionRangeXP(input, input.value.length, input.value.length);
            //e.preventDefault();
            
        } else if (e.keyCode === 40) { // Down
            
            if (++command_index >= command_history.length) {
                command_index = command_history.length;
                input.value = "";
            } else {
                input.value = command_history[command_index];
            }
            setSelectionRangeXP(input, input.value.length, input.value.length);
            //e.preventDefault();
            
        } /*else if (e.keyCode === 46 && e.shiftKey) { // Shift+Delete
            
            if (input.value === command_history[command_index]) {
                command_history.splice(command_index, 1);
                command_index = Math.max(0, command_index - 1)
                input.value = command_history[command_index] || "";
                save_command_history();
            }
            //e.preventDefault();
            
        }*/
    });

    this.element = console_element;
    this.input = input;
    this.addButton = add_button;
    this.addPopupButton = add_popup_button;
    this.addPopupMenuButton = add_popup_menu_button;

    this.handleUncaughtErrors = function() {
        window.onerror = error;
    };

    this.log = log;
    this.logHTML = logHTML;
    this.error = error;
    this.warn = warn;
    this.info = info;
    this.getLastEntry = get_last_entry;
    this.clear = clear;
}
