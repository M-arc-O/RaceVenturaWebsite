import { AfterViewInit, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { CarouselService } from 'src/app/components/carousel/carousel.service';
import { ComponentBase, UserService } from 'src/app/shared';
import { TeamResultViewModel } from '../../shared/models';
import { resultStateSelector } from '../../store';
import * as raceActions from '../../store/actions/race.actions';
import * as L from 'leaflet';

@Component({
    selector: 'app-race-map',
    templateUrl: './race-map.component.html',
    styleUrls: ['./race-map.component.css']
})
export class RaceMapComponent extends ComponentBase implements OnInit, AfterViewInit {
    private raceId: string;
    private map: L.Map;
    private colors = ['blue', 'gold', 'red', 'green', 'orange', 'yellow', 'violet', 'grey', 'black'];
    private stageGroup: L.LayerGroup;
    private legend: L.Control;

    public raceResult$: Observable<TeamResultViewModel[]>;
    public selectedTeamNumber: number;

    constructor(
        private store: Store<TeamResultViewModel[]>,
        private route: ActivatedRoute,
        private carouselService: CarouselService,
        userService: UserService,
        router: Router) {
        super(userService, router);
        this.carouselService.showCarousel$.next(false);
        this.raceResult$ = this.store.pipe(select(resultStateSelector));
    }

    public ngOnInit(): void {
        this.route.params.pipe(takeUntil(this.unsubscribe$)).subscribe(params => {
            this.raceId = params['id'];
            this.store.dispatch(new raceActions.GetRaceResultAction(this.raceId));
        });
    }

    public ngAfterViewInit(): void {
        this.initMap();
    }

    public refresh(): void {
        this.store.dispatch(new raceActions.GetRaceResultAction(this.raceId));
    }

    public detailsClicked(result: TeamResultViewModel) {
        if (this.selectedTeamNumber !== result.teamNumber) {
            this.selectedTeamNumber = result.teamNumber;
            this.addSampleMarker(result);
        }
    }

    private initMap(): void {
        this.map = L.map('map', {
            center: [51.977205, 4.535121],
            zoom: 12
        });

        const osm = L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            maxZoom: 19,
            attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
        });

        osm.addTo(this.map);
        
        const mqi = L.tileLayer('http://api.mapbox.com/v4/mapbox.satellite/{z}/{x}/{y}.jpg90?access_token=pk.eyJ1IjoibS1hcmMtbyIsImEiOiJja21ld2k5ZDEwMXZ6MndrYjU1ejduNzIxIn0.eNEQaWWWAm1iFa0KOJechA', {
            attribution: '&copy; <a href="https://www.mapbox.com/">Mapbox</a>',
            maxZoom: 19
        });

        var baseMaps = {
            "OpenStreetMap": osm,
            "MapQuestImagery": mqi
        };

        L.control.layers(baseMaps, {}, { position: 'topleft' }).addTo(this.map);
    }

    private addSampleMarker(result: TeamResultViewModel) {
        if (this.stageGroup !== undefined) {
            this.map.removeLayer(this.stageGroup);
        }        
        
        if (this.legend !== undefined) {
            this.map.removeControl(this.legend);
        }

        var div = L.DomUtil.create("div", "legend");
        div.innerHTML += "<h4>Legend</h4>";
        div.innerHTML += "<table>";

        let index = 0;
        this.stageGroup = L.layerGroup();
        result.stageResults.forEach(stage => {
            let color: string;
            ({ color, index } = this.getColor(index));
            let iconUrl = `https://raw.githubusercontent.com/pointhi/leaflet-color-markers/master/img/marker-icon-${color}.png`;
            div.innerHTML += `<i class="icon" style="background-image: url(${iconUrl})"></i><span>${stage.stageNumber} - ${stage.stageName}</span>`;

            stage.pointResults.forEach(point => {
                let marker = new L.Marker([point.latitude, point.longitude])
                    .setIcon(
                        L.icon({
                            iconUrl: iconUrl,
                            shadowUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/0.7.7/images/marker-shadow.png',
                            iconSize: [25, 41],
                            iconAnchor: [13, 41],
                        }))
                    .bindTooltip(`${point.name} (${point.value})`);
                this.stageGroup.addLayer(marker);                
            });
        });

        this.stageGroup.addTo(this.map);
        
        this.legend = new L.Control({ position: 'topright' });
        this.legend.onAdd = function (map) {
            return div;
        };
        this.legend.addTo(this.map);
    }

    private getColor(index: number) {
        let color = this.colors[index];
        index += 1;

        if (index >= this.colors.length) {
            index = 0;
        }
        return { color, index };
    }
}