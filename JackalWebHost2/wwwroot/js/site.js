// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

/// собственно объект игры
var game = new Game();

$(document).ready(function () {
    /// Быстрое начало игры
    game.MakeStart(uuidGen(), ['human', 'robot2', 'robot2', 'robot2']);

    /// выбор игроков
    $('#player-choose-menu div').click(function () {
        if ($(this).hasClass('human')) {
            $(this).removeClass();
            $(this).addClass('robot');
        } else if ($(this).hasClass('robot')) {
            $(this).removeClass();
            $(this).addClass('robot2');
        } else {
            $(this).removeClass();
            $(this).addClass('human');
        }
    });

    /// Начинаем игру со своими настройками
    $('#submit-button-settings').click(function () {
        game.GameReset();

        var players = $('#player-choose-menu div');
        var input = [];
        for (var i = 0; i < players.length; i++) {
            input.push($(players[i]).attr('class'));
        }
        var mapId = $('#game-map-id').val();
        game.MakeStart(uuidGen(), input, parseInt(mapId));

        $("#preparing-file-modal").modal('toggle');
    });
    /// Закрываем окно с настройками
    $('#reset-button-settings').click(function () {
        $("#preparing-file-modal").modal('toggle');
    });

    /// Смена режима игры (пошаговый/непрерывный)
    $('#game').click(function () {
        game.ChangeState();
    });

    /// Настроить игру
    $('#game-settings').click(function () {
        $("#preparing-file-modal").modal('toggle');
        return false;
    });

    /// Начать заново
    $('#reset').click(function () {
        game.GameReset();
        return false;
    });

    /// Следующий ход в пошаговом режиме
    $('#next').click(function () {
        game.MakeTurn(game.gameName);
    });

    /// Атрибут хода (с монетой/ без монеты)
    $('#with-coin').change(function () {
        game.moves.DrawPirateAvailableMoves($('.photo-active').data('value'));

        //if (!game.isHumanStep) return;
        //game.moves.currentTeam.withCoin = $(this).is(':checked');
    });

    /// Выбор пирата, ходы которого будут подсвечиваться и,
    /// если игрок - человек, который будет делать ход
    $('.bs-photos').click(function () {

        $('.bs-photos').removeClass('photo-active');
        $(this).addClass('photo-active');

        $('#with-coin').prop('checked', true);
        game.moves.DrawPirateAvailableMoves($(this).data('value'));

        if (!game.isHumanStep) return;
        game.moves.currentTeam.pirate = $(this).data('value');
    });
});

function Game() {

    this.gameName = null;
    this.gamestate = 'stop';
    this.map;
    this.groups;
    this.isHumanStep = false;

    // подсветка ходов или выбор хода
    this.moves;

    this.ChangeState = function () {

        if (this.gamestate == 'stop') {

            this.GameStart();

        } else if (this.gamestate == 'start') {

            this.GameStop();
        }
    }

    this.GameStart = function () {

        this.gamestate = 'start';
        $('#game').text('Остановить');
        setTimeout(this.GameTurn, 250);
    }

    this.GameStop = function () {

        if (this.isHumanStep) {
            this.gamestate = 'stop';
            $('#game').text('Продолжить');
        }
        else {
            this.gamestate = 'stoping';
            $('#game').text('Ожидаем ...');
        }
    }

    this.GameTurn = function () {

        if (this.gamestate == 'stoping') {

            this.gamestate = 'stop';
            $('#game').text('Продолжить');
            return;
        }

        game.MakeTurn(game.gameName);
    }

    this.MakeStart = function (gameName, players, mapId) {

        var self = this;
        $.post("/Game/Start", {gameName: gameName, settings: JSON.stringify({players : players, mapId: mapId}) }, function (data) {

            self.gameName = data.gameName;
            $('#gamename').text(self.gameName);
            $('#mapcode').text(data.mapId);

            $('.bs-panel-statistics').show();
            $('.bs-panel-control').show();

            $('#end-game-alert').hide();

            $('.controls').removeClass('center-block').addClass('col-md-3')
            $('#map').addClass('col-md-7');

            self.groups = new Teams(data.stat.Teams);

            self.map = new Map(data.map.Width, data.map.Height);
            self.map.RedrawMap(data.map.Changes);

            self.moves = new Moves(self.map);

            self.GameStart();

        });
    }

    this.MakeTurn = function(gameName, moveNum) {

        var self = this;
        $.post("/Game/Turn", {gameName: gameName, turnNum: moveNum }, function (data) {

            if (data.stat.IsGameOver) {

                self.groups.Redraw(data.stat.Teams)
                $('#counter').text(data.stat.TurnNo);
                var elm = $('#end-game-alert');
                elm.text('Игра закончена. Победил ' + self.groups.GetWinner(data.stat.Teams));
                elm.show();
                return;
            }

            self.moves.UndrawMoves();

            self.map.RedrawMap(data.changes);
            self.groups.Redraw(data.stat.Teams)
            $('#counter').text(data.stat.TurnNo);

            self.isHumanStep = data.stat.IsHumanPlayer;

            self.moves.SetCurrentTeam(data.stat.CurrentTeamId);
            self.moves.DrawAvailablePirates(data.moves);

            if (!data.stat.IsHumanPlayer && (self.gamestate == 'start')) {
                setTimeout(self.GameTurn, 150);
            }
        });
    }

    this.GameReset = function() {
        if (this.gamestate == 'start') {

            if (this.isHumanStep) {
                this.gamestate = 'stop';
                $('#game').text('Продолжить');
            }
            else {
                this.gamestate = 'stoping';
                $('#game').text('Ожидаем');
            }
            setTimeout(this.GameReset, 500);
        }
        else {
            $.post("/Game/Reset", null, function (data) {

                self.gamename = null;
                $('#gamename').text('');
                $('#game').text('Начать');

                $('.bs-panel-statistics').hide();
                $('.bs-panel-control').hide();
                $('#end-game-alert').hide();

                $('.controls').removeClass('col-md-3').addClass('center-block')
                $('#map').removeClass('col-md-7');

                $('#teams').empty();

                var mapElm = $('#map');
                if (typeof (mapElm) != 'undefined')
                    mapElm.html('');

                game.MakeStart(uuidGen(), ['human', 'robot2', 'robot2', 'robot2']);
            });
        }
    }
}


/// ==============================
/// Обработчик щелчка пользователя
/// ==============================
var makeUserHandler = function (i, j) {
    return function () {
        if (!game.isHumanStep) return;

        var action = game.moves.GetActionMove(i, j);
        if (action != null) {
            game.MakeTurn(game.gameName, action.MoveNum);
        }
        //else alert('нет хода');
    };
};


/// ===========================
/// Отображение карты на экране
/// ===========================
function Map(width, height) {

    var mapElm = $('#map');
    if (typeof (mapElm) == 'undefined') return;

    this.width = width;
    this.height = height;
    this.fields = {};

    for (var i = 0; i < this.width; i++) {
        this.fields[i] = {};
        for (var j = 0; j < this.height; j++) {
            this.fields[i][j] = { isUnknown: true };
        }
    }

    var table = $('<table style="border: 1px black solid;"></table>');
    mapElm.append(table);
    for (var j = (this.height - 1); j >= 0 ; j--) {
        var tr = $('<tr></tr>');
        table.append(tr);
        for (var i = 0; i < this.width; i++) {
            var td = $('<td></td>');
            var back = $('<div class="back" />');

            // обработчик щелчка пользователя
            back.click(makeUserHandler(i, j));

            var lev = $('<div class="level" />');
            var coins = $('<div class="coins" />');
            var pirates = $('<div class="pirates" />');
            td.append(lev);
            td.append(back);
            lev.append(coins);
            lev.append(pirates);
            this.fields[i][j].location = td;
            this.fields[i][j].locsize = 1;
            this.fields[i][j].back = back;
            this.fields[i][j][0] = {
                location: lev,
                coins: coins,
                pirates: pirates
            };
            tr.append(td);
        }
    }

    /// перерисовка карты
    this.RedrawMap = function (changes) {

        for (var index in changes) {

            var each = changes[index];
            var field = this.fields[each.X][each.Y];

            if (!each.IsUnknown && field.isUnknown) {

                field[0].location.addClass('level-' + each.Levels.length + '0');

                field.isUnknown = false;
                if (each.Levels.length > 1) {

                    for (var lindex in each.Levels) {
                        if (lindex > 0) {
                            var lev = $('<div class="level-' + each.Levels.length + lindex + '" />');
                            var coins = $('<div class="coins" />');
                            var pirates = $('<div class="pirates" />');
                            field.location.append(lev);
                            field.locsize = each.Levels.length;
                            lev.append(coins);
                            lev.append(pirates);
                            field[lindex] = {
                                location: lev,
                                coins: coins,
                                pirates: pirates
                            };
                        }
                    }
                }
            }

            if (each.BackgroundImageSrc != null)
                field.back.css('backgroundImage', 'url(' + each.BackgroundImageSrc + ')');
            else {
                field.back.css('backgroundImage', '');
                field.back.css('backgroundColor', each.BackgroundColor);
            }

            if (each.Rotate == 1) {
                field.back.css('transform', 'rotate(90deg)');
            } else if (each.Rotate == 2) {
                field.back.css('transform', 'rotate(180deg)');
            } else if (each.Rotate == 3) {
                field.back.css('transform', 'rotate(270deg)');
            }

            for (var lindex in each.Levels) {

                var lev = each.Levels[lindex];

                var level = field[lindex];
                level.pirates.css('backgroundColor', lev.hasPirates ? lev.Pirate.BackColor : '');
                level.pirates.text(lev.hasPirates ? lev.Pirate.Text : '');

                level.coins.css('backgroundColor', lev.hasCoins ? lev.Coin.BackColor : '');
                level.coins.text(lev.hasCoins ? lev.Coin.Text : '');
            }

        }
    }

}

/// ================================================================
/// Отображение подсветки ходов и совершение действий human player-а
/// ================================================================
function Moves(map) {

    this.moves = [];
    this.teamsHistory = {};
    this.currentTeam;
    this.map = map;

    this.SetCurrentTeam = function (teamIndex) {

        var team = this.teamsHistory[teamIndex];
        if (team == null) {
            team = {
                pirate: 1
            }
            this.teamsHistory[teamIndex] = team;
        }
        this.currentTeam = team;
    }

    this.DrawAvailablePirates = function(avmoves) {

        this.moves = avmoves;

        // ищем пиратов, могущих сделать ход
        var nums = 0;
        for (var index = 0 ; index < avmoves.length ; index++) {

            var move = avmoves[index];
            if (move.From.PirateNum == 3)
                nums |= 4;
            nums |= move.From.PirateNum;
        }

        // отображаем пиратов, могущих сделать ход
        var availables = [nums & 1, nums & 2, nums & 4];
        var pirateElms = [$('#first-pirate'), $('#second-pirate'), $('#third-pirate')];
        for (index = 0 ; index < 3 ; index++) {
            if (availables[index])
                pirateElms[index].show();
            else pirateElms[index].hide();
        }

        var currentPirate = (availables[this.currentTeam.pirate - 1]) ? this.currentTeam.pirate : 1;

        // выставляем настройки для ходов выбранного пирата
        $('.bs-photos').removeClass('photo-active');
        $('.bs-photos[data-value=' + currentPirate + ']').addClass('photo-active');

        // отображение подсветки ходов выбранного пирата
        $('#with-coin').prop('checked', true);
        this.DrawPirateAvailableMoves(currentPirate);
    }
    
    this.DrawPirateAvailableMoves = function(num) {

        var withCoin = false;
        for (index = 0 ; index < this.moves.length ; index++) {
            var mv = this.moves[index];
            if ((mv.From.PirateNum == num) && (mv.WithCoin)) {
                withCoin = true;
                break;
            }
        }

        if (withCoin) {
            withCoin = $('#with-coin').prop('checked');
            $('#with-coin-checkbox').show();
        } else {
            $('#with-coin').prop('checked', false);
            $('#with-coin-checkbox').hide();
        }

        for (var index = 0; index < this.moves.length; index++) {

            var move = this.moves[index];
            var fld = this.map.fields[move.To.X][move.To.Y];

            if (fld.locsize == 1) {
                fld.back.css('opacity', '1');
            } else {
                fld.location.removeClass('digit_' + (fld.locsize - move.To.Level));
                fld.back.css('opacity', '1');
            }
        }

        //alert('num = ' + num);
        for (index = 0 ; index < this.moves.length ; index++) {

            move = this.moves[index];
            fld = this.map.fields[move.To.X][move.To.Y];
            var level = fld[move.To.Level];

            if (move.From.PirateNum == num) {

                if (withCoin && !move.WithCoin)
                    continue;

                if (fld.locsize == 1) {
                    fld.back.css('opacity', '0.5');
                } else {
                    fld.location.addClass('digit_' + (fld.locsize - move.To.Level));
                    fld.back.css('opacity', '0.5');
                }
            }

        }
    }

    this.UndrawMoves = function() {

        while (this.moves.length > 0) {
            var move = this.moves.pop();
            var fld = this.map.fields[move.To.X][move.To.Y];
            if (fld.locsize == 1) {
                fld.back.css('opacity', '1');
            } else {
                fld.location.removeClass('digit_' + (fld.locsize - move.To.Level));
                fld.back.css('opacity', '1');
            }
        }
    }

    this.GetActionMove = function (i, j) {

        for (var index = 0; index < this.moves.length; index++) {

            var move = this.moves[index];
            var curpirate = $('.photo-active').data('value');
            if ((move.To.X == i) && (move.To.Y == j) && (curpirate == move.From.PirateNum)) {

                // есть ход
                if ($('#with-coin').prop('checked') && !move.WithCoin)
                    continue;

                return move;
            }
        }
        return null;
    }

}

/// ===========================================
/// Отображение статистики по игрокам на экране
/// ===========================================
function Teams(teams) {

    var teamsElm = $('#teams');
    if (typeof (teamsElm) == 'undefined') return;

    this.teams = {};

    for (var index in teams) {

        var team = teams[index];
        var row = $('<div class="row" />');

        var name = $('<div class="col-md-8" />');
        name.text(team.name);
        name.css('backgroundColor', team.backcolor);
        row.append(name);

        var elm = $('<div class="col-md-4" />');
        elm.css('backgroundColor', team.backcolor);
        var gld = $('<span class="badge"></span>');
        gld.text(team.gold);
        elm.append(gld);
        row.append(elm);

        this.teams[team.id] = {
            elm: name,
            gld: gld
        };
        teamsElm.append(row);
    }

    // Перерисовка измененной статистики по игрокам
    this.Redraw = function(teams) {

        for (var id in teams) {
            var team = teams[id];
            this.teams[team.id].gld.text(team.gold);
        }
    }

    // Определение победителя по статистике
    this.GetWinner = function (teams) {

        var maxgold = 0;
        var winner;

        for (var id in teams) {
            var team = teams[id];
            if (team.gold > maxgold) {
                maxgold = team.gold;
                winner = team.name;
            } else if (team.gold == maxgold) {
                winner += (" и " + team.name);
            }
        }
        return winner;
    }
}

function uuidGen() {
    return "10000000-1000-4000-8000-100000000000".replace(/[018]/g, c =>
        (+c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> +c / 4).toString(16)
    );
}
