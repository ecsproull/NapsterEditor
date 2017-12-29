var trackPlaying;
function play(track) {
    trackPlaying = track;
    Napster.player.play(track);
}

function stopPlayer() {
    Napster.player.pause();
}

function load(clientId, token, refresh) {
    Napster.init({
        consumerKey: localStorage.getItem("clientId")
    });

    Napster.player.on('ready', function (e) {
        Napster.member.set({
            accessToken: token,
            refreshToken: refresh
        });
        Napster.player.auth();
    });

    Napster.player.on('playevent', function (e) {
        if (e.data.code == 'PlayComplete') {
            $.get("/Playlists/GetNextTrack", { currentTrack: trackPlaying }, function (data) {
                play(data);
            });
        }
    });
}