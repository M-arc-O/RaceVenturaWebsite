import { environment } from "src/environments/environment";


export class ConfigurationService {
    static readonly ApiRoot = environment.apiBaseUrl;
}
