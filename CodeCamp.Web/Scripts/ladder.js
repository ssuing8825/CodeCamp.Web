$(function () {

    // this is the player type stored in the collection
    function player(Id, Name, Rank, SkillLevel, ownerViewModel) {
        this.Id = Id;
        this.Name = ko.observable(Name);
        this.Rank = ko.observable(Rank);
        this.SkillLevel = ko.observable(SkillLevel);

        this.update = function () {

            ownerViewModel.saving(true);
            var playerSelf = this;
            $.ajax({
                url: '/Players/' + Id,
                type: 'PUT',
                dataType: 'json',
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON(playerSelf),
                success: function () {
                    //Could do something on success.
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(textStatus);
                },
                complete: function () {
                    ownerViewModel.saving(false);
                    ownerViewModel.saved(true);
                }
            });

        };

        this.remove = function () {
            ownerViewModel.saving(true);

            var playerSelf = this;

            $.ajax({
                url: '/Players/' + Id,
                type: 'DELETE',
                success: function () {
                    ownerViewModel.players.remove(playerSelf);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(textStatus);
                },
                complete: function () {
                    ownerViewModel.saving(false);
                    ownerViewModel.saved(true);
                }
            });
        };
    }


    // This is the list view model
    function playerListViewModel() {
        this.players = ko.observableArray([]);
        this.saving = ko.observable(false);
        this.saved = ko.observable(false);

        // Load initial state from server
        var self = this;

        $.ajax({
            url: '/Players',
            dataType: 'json',
            type: 'GET',
            success: function (data) {
                var mappedPlayers = $.map(data, function (item) {
                    return new player(item.Id, item.Name, item.Rank, item.SkillLevel, self)
                });
                self.players(mappedPlayers);

            }
        });
    }

    var localViewModel = new playerListViewModel()
    ko.applyBindings(localViewModel);


    // validation

    var Name = $("#Name"),
			Rank = $("#Rank"),
			SkillLevel = $("#SkillLevel"),
			allFields = $([]).add(Name).add(Rank).add(SkillLevel),
			tips = $(".validateTips");

    function updateTips(t) {
        tips
				.text(t)
				.addClass("ui-state-highlight");
        setTimeout(function () {
            tips.removeClass("ui-state-highlight", 1500);
        }, 500);
    }

    function checkLength(o, n, min, max) {
        if (o.val().length > max || o.val().length < min) {
            o.addClass("ui-state-error");
            updateTips("Length of " + n + " must be between " + min + " and " + max + ".");
            return false;
        } else {
            return true;
        }
    }

    function checkRegexp(o, regexp, n) {
        if (!(regexp.test(o.val()))) {
            o.addClass("ui-state-error");
            updateTips(n);
            return false;
        } else {
            return true;
        }
    }
    // Add Dialog
    $("#dialog-form").dialog({
        autoOpen: false,
        height: 300,
        width: 350,
        modal: true,
        buttons: {
            "Create a player": function () {
                var bValid = true;
                allFields.removeClass("ui-state-error");

                bValid = bValid && checkLength(Name, "Name", 3, 16);
                bValid = bValid && checkLength(Rank, "Rank", 1, 6);
                bValid = bValid && checkLength(SkillLevel, "SkillLevel", 1, 1);

                bValid = bValid && checkRegexp(Name, /^[a-z]([0-9a-z_ ])+$/i, "Player may consist of a-z, 0-9, underscores, begin with a letter.");
                bValid = bValid && checkRegexp(Rank, /^[0-9]+$/i, "Ramk must be a number.");


                //This is using forms encoding but could be JSON only
                if (bValid) {
                    $.ajax({
                        url: '/Players',
                        type: 'POST',
                        //dataType: 'json', This doesn't make a difference since JSON is the default returned. 
                        //contentType: "application/json; charset=utf-8",
                        //data: JSON.stringify(Players),

                        data: $('#addPlayerForm').serialize(), // Just send is forms URL encoding.
                        success: function (data) {
                            localViewModel.players.push(new player(data.Id, data.Name, data.Rank, data.SkillLevel, localViewModel))
                        }
                    });

                    $(this).dialog("close");
                }
            },
            Cancel: function () {
                $(this).dialog("close");
            }
        },
        close: function () {
            allFields.val("").removeClass("ui-state-error");
        }
    });


    $("#create-user")
			.button()
			.click(function () {
			    $("#dialog-form").dialog("open");
			});

});