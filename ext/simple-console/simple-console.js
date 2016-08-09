
var SimpleConsole = function(options) {

	if (!options.handleCommand) {
		throw new Error("options.handleCommand is required");
	}

	var handle_command = options.handleCommand;
	var placeholder = options.placeholder || "";
	var autofocus = options.autofocus;
	var storage_id = options.storageID || "simple-console";

	var add_svg = function(to_element, icon_class_name, svg, viewBox = "0 0 16 16") {
		var icon = document.createElement("span");
		icon.className = icon_class_name;
		icon.innerHTML = '<svg width="1em" height="1em" viewBox="' + viewBox + '">' + svg + '</svg>';
		to_element.insertBefore(icon, to_element.firstChild);
	};

	var add_chevron = function(to_element) {
		add_svg(to_element, "input-chevron",
			'<path d="M6,4L10,8L6,12" fill="none" stroke="currentColor" stroke-width="1.5" stroke-linecap="round"></path>'
		);
	};

	var add_command_history_icon = function(to_element) {
		add_svg(to_element, "command-history-icon",
			'<path style="fill:currentColor" d="m 44.77595,87.58531 c -5.22521,-1.50964 -12.71218,-5.59862 -14.75245,-8.05699 -1.11544,-1.34403 -0.96175,-1.96515 1.00404,-4.05763 2.86639,-3.05114 3.32893,-3.0558 7.28918,-0.0735 18.67347,14.0622 46.68328,-0.57603 46.68328,-24.39719 0,-16.97629 -14.94179,-31.06679 -31.5,-29.70533 -14.50484,1.19263 -25.37729,11.25581 -28.04263,25.95533 l -0.67995,3.75 6.6362,0 6.6362,0 -7.98926,8 c -4.39409,4.4 -8.35335,8 -8.79836,8 -0.44502,0 -4.38801,-3.6 -8.7622,-8 l -7.95308,-8 6.11969,0 6.11969,0 1.09387,-6.20999 c 3.5237,-20.00438 20.82127,-33.32106 40.85235,-31.45053 11.43532,1.06785 21.61339,7.05858 27.85464,16.39502 13.06245,19.54044 5.89841,45.46362 -15.33792,55.50045 -7.49404,3.54188 -18.8573,4.55073 -26.47329,2.35036 z m 6.22405,-32.76106 c 0,-6.94142 0,-13.88283 0,-20.82425 2,0 4,0 6,0 0,6.01641 0,12.03283 0,18.04924 4.9478,2.93987 9.88614,5.89561 14.82688,8.84731 l -3.27407,4.64009 c -5.88622,-3.5132 -11.71924,-7.11293 -17.55281,-10.71239 z"/>',
			"0 0 102 102"
		);
	};

	var console_element = document.createElement("div");
	console_element.className = "simple-console";

	var output = document.createElement("div");
	output.className = "simple-console-output";
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
		button.addEventListener("click", action);
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
		add_command_history_icon(popup_button);

		input_wrapper.appendChild(popup_button);
		input_wrapper.appendChild(popup);

		popup_button.addEventListener("keydown", function(e) {
			if (e.keyCode === 40) { // Down
				e.preventDefault();
				first_item = popup.querySelector("[tabindex='0']");
				first_item.focus();
			}
		});

		popup.addEventListener("keydown", function(e) {
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

		popup_button.addEventListener("click", toggle_popup);

		addEventListener("mousedown", function(e) {
			if (popup_is_open()) {
				if (!(
					e.target.closest(".popup-button") == popup_button ||
					e.target.closest(".popup-menu") == popup
				)) {
					close_popup();
				}
			}
		});

		addEventListener("focusin", function(e) {
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
					divider.classList.add("menu-divider");
					menu.appendChild(divider);
				} else {
					var menu_item = document.createElement("div");
					menu_item.classList.add("menu-item");
					menu_item.setAttribute("tabindex", 0);
					menu_item.addEventListener("click", item.action);
					menu_item.textContent = item.label;
					menu.appendChild(menu_item);
				}
			}
		});

		var menu = popup_button.popup;

		menu.addEventListener("click", function(e) {
			var menu_item = e.target.closest(".menu-item");
			if (menu_item) {
				popup_button.closePopup();
			}
		});

		menu.addEventListener("keydown", function(e) {
			if (e.keyCode === 38) { // Up
				e.preventDefault();
				var prev = document.activeElement.previousElementSibling;
				while (prev && prev.nodeName === "HR") {
					prev = prev.previousElementSibling;
				}
				if (prev && prev.classList.contains("menu-item")) {
					prev.focus();
				}
			} else if (e.keyCode === 40) { // Down
				e.preventDefault();
				var next = document.activeElement.nextElementSibling;
				while (next && next.nodeName === "HR") {
					next = next.nextElementSibling;
				}
				if (next && next.classList.contains("menu-item")) {
					next.focus();
				}
			} else if (e.keyCode === 13 || e.keyCode === 32) { // Enter or Space
				e.preventDefault();
				document.activeElement.click();
			}
		});

		return popup_button;
	};

	addEventListener("keydown", function(e){
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

	add_popup_menu_button(function() {
		var items = [];

		if (command_history.length > 0) {
			for (var i = 0; i < command_history.length; i++) {
				var command = command_history[i];
				(function(command, i) {
					items.push({
						label: command,
						action: function() {
							input.value = command;
							input.focus();
							input.setSelectionRange(input.value.length, input.value.length);
						}
					});
				}(command, i));
			}

			items.push({
				type: "divider"
			});

			items.push({
				label: "Clear command history",
				action: clear_command_history
			});
		} else {
			items.push({
				label: "Command history empty",
				action: function() {}
			});
		}

		return items;
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
		if (content instanceof Element) {
			entry.appendChild(content);
		} else {
			entry.innerText = entry.textContent = content;
		}
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
		get_last_entry().classList.add("error");
	};

	var warn = function(content) {
		log(content);
		get_last_entry().classList.add("warning");
	};

	var info = function(content) {
		log(content);
		get_last_entry().classList.add("info");
	};

	output.is_scrolled_to_bottom = function() {
		return output.scrollTop + output.clientHeight >= output.scrollHeight;
	};

	output.scroll_to_bottom = function() {
		output.scrollTop = output.scrollHeight;
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

	input.addEventListener("keydown", function(e) {
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
			command_entry.classList.add("input");
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
			input.setSelectionRange(input.value.length, input.value.length);
			e.preventDefault();
			
		} else if (e.keyCode === 40) { // Down
			
			if (++command_index >= command_history.length) {
				command_index = command_history.length;
				input.value = "";
			} else {
				input.value = command_history[command_index];
			}
			input.setSelectionRange(input.value.length, input.value.length);
			e.preventDefault();
			
		} else if (e.keyCode === 46 && e.shiftKey) { // Shift+Delete
			
			if (input.value === command_history[command_index]) {
				command_history.splice(command_index, 1);
				command_index = Math.max(0, command_index - 1)
				input.value = command_history[command_index] || "";
				save_command_history();
			}
			e.preventDefault();
			
		}
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

};
