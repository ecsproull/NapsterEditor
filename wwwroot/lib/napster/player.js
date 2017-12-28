function playerReady() {
    Player.playerReady()
    alert("asdfas");
}
var Player = {
    accessToken: null,
    apiKey: null,
    session: null,
    player: null,
    duration: null,
    trackId: null,
    paused: !1,
    volume: 1,
    streamStarted: !1,
    napiUrl: "https://api.napster.com",
    loadSession: function (t, a) {
        if (this.session) return void t();
        var e = {
            type: "POST",
            dataType: "json",
            url: this.napiUrl + "/v1/sessions",
            headers: {
                Authorization: "Bearer " + this.accessToken
            }
        };
        $.ajax(e).done(function (a) {
            this.session = a.id, t()
        }.bind(this)).fail(function (t) {
            a(t)
        })
    },
    loadTrack: function (t, a, e) {
        var i = {
            dataType: "json",
            url: this.napiUrl + "/v1/tracks/" + t + "?apikey=" + this.apiKey,
            headers: {
                Authorization: "Bearer " + this.accessToken
            }
        };
        $.ajax(i).done(function (t) {
            a(t)
        }).fail(function (t) {
            e(t)
        })
    },
    loadStream: function (t, a, e) {
        var i = {
            dataType: "json",
            url: this.napiUrl + "/v1/play/" + t + "?web=true&context=ON_DEMAND&sessionId=" + this.session,
            headers: {
                Authorization: "Bearer " + this.accessToken
            }
        };
        $.ajax(i).done(function (t) {
            a(t.url)
        }).fail(function (t) {
            e(t)
        })
    },
    notify: function (t, a) {
        window.parent.postMessage({
            type: t,
            data: a
        }, "*")
    },
    onTrackFail: function (t) {
        var a, e, i;
        t.responseText && (a = JSON.parse(t.responseText), e = a && void 0 != a.message ? a.message : t.responseText, i = a && void 0 != a.code ? a.code : "UnhandledError"), this.paused = !0, this.player.pause(), 403 === t.status && -1 != e.indexOf("Playback session expired by another client") ? (this.session = null, this.notify("playsessionexpired")) : 404 === t.status && -1 != e.indexOf("Unknown Resource") ? (this.session = null, this.playTrack(this.trackId)) : this.notify("error", {
            code: i,
            detail: e
        })
    },
    init: function () {
        this.volume = Number(localStorage.volume) || 1, window.addEventListener("message", function (t) {
            var a = t.data.method,
                e = t.data.args;
            switch (a) {
                case "auth":
                    this.accessToken = e.accessToken, this.apiKey = e.consumerKey, this.playbackStoppedEvent();
                    break;
                case "play":
                    var i = e;
                    if (this.trackId == i && this.paused) return this.paused = !1, void this.player.resume();
                    this.trackId = i, this.playTrack(i);
                    break;
                case "pause":
                    this.paused = !0, this.player.pause();
                    break;
                case "seek":
                    this.paused = !1, this.player.seek(e);
                    break;
                case "setVolume":
                    this.volume = e, localStorage.volume = e, this.player.setVolume(e)
            }
        }.bind(this))
    },
    playTrack: function (t) {
        this.loadSession(function () {
            this.loadTrack(t, function (a) {
                this.duration = a.duration, this.loadStream(t, function (t) {
                    this.player.setVolume(this.volume), this.paused = !1, this.player.play(t), this.notify("metadata", a)
                }.bind(this), function (t) {
                    this.onTrackFail(t)
                }.bind(this))
            }.bind(this), function (t) {
                this.onTrackFail(t)
            }.bind(this))
        }.bind(this), function (t) {
            this.onTrackFail(t)
        }.bind(this))
    },
    onTimeUpdate: function () {
        var t = this.player.time();
        this.updatePlaybackEvent(t), this.notify("playtimer", {
            currentTime: t,
            totalTime: this.duration
        })
    },
    onError: function () { },
    onStatus: function (t) {
        switch (t) {
            case "NetConnection.Connect.Success":
                this.streamStarted = !1, this.notify("playevent", {
                    id: this.trackId,
                    code: "Connected"
                });
                break;
            case "NetStream.Buffer.Full":
                this.notify("playevent", {
                    id: this.trackId,
                    code: "BufferFull"
                });
                break;
            case "NetStream.Pause.Notify":
                this.notify("playevent", {
                    id: this.trackId,
                    code: "Paused"
                });
                break;
            case "NetStream.Unpause.Notify":
                this.notify("playevent", {
                    id: this.trackId,
                    code: "Unpaused"
                });
                break;
            case "NetStream.Play.Start":
                this.streamStarted || (this.notify("playevent", {
                    id: this.trackId,
                    code: "PlayStarted"
                }), this.streamStarted = !0, this.playbackStartedEvent());
                break;
            case "NetStream.Play.Complete":
                this.notify("playevent", {
                    id: this.trackId,
                    code: "PlayComplete"
                }), this.playbackStoppedEvent(), this.notify("playstopped")
        }
    },
    playerReady: function () {
        this.player = $("#swf object")[0], this.player.addEventListener("timeupdate", "Player.onTimeUpdate"), this.player.addEventListener("error", "Player.onError"), this.player.addEventListener("status", "Player.onStatus"), this.notify("ready", {})
    },
    playbackStartedEvent: function () {
        var t = {
            playback: {
                id: this.trackId,
                format: "MP3 64",
                bitrate: 128
            }
        };
        localStorage.playbackNotification = JSON.stringify(t), t.type = "playbackStart", t.playback.started = (new Date).toISOString(), this.sendEvent(t)
    },
    playbackStoppedEvent: function () {
        var t = localStorage.playbackNotification;
        if (t) {
            var a = JSON.parse(t);
            a && (a.type = "playbackStop", a.playback.started = (new Date).toISOString(), this.sendEvent(a), localStorage.removeItem("playbackNotification"))
        }
    },
    updatePlaybackEvent: function (t) {
        var a, e, e = localStorage.playbackNotification;
        if (e) {
            var i = JSON.parse(e);
            if (i) {
                var a = Math.floor(t);
                i.duration !== a && (i.duration = a, localStorage.playbackNotification = JSON.stringify(i))
            }
        }
    },
    sendEvent: function (t) {
        var a = {
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(t),
            url: this.napiUrl + "/v1/events?apikey=" + this.apiKey,
            headers: {
                Authorization: "Bearer " + this.accessToken
            }
        };
        $.ajax(a).done(function (t) { })
    }
};
Player.init();// JavaScript source code
