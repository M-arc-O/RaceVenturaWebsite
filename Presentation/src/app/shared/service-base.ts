import { HttpParams } from '@angular/common/http';

export abstract class ServiceBase {
    protected getHttpOptions(additionalParams?: { key: string, value: string }[]): { params: HttpParams } {
        let params = new HttpParams();
        if (additionalParams !== undefined) {
            for (const header of additionalParams) {
                params = params.append(header.key, header.value);
            }
        }

        return { params: params };
    }
}
