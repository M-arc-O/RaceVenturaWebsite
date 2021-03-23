import { Byte } from "@angular/compiler/src/util";
import { TeamCategory } from "./team-category";

export class TeamStoreModel {
    public teamId: string;
    public raceId: string;
    public number: number;
    public name: string;
    public category: TeamCategory;
    public finishTime: Date;
    public qrCodeArray: Byte[];
}
