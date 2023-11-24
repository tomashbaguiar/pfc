import { AxiosInstance } from "axios";
import bunyan from "bunyan";

export class TemplateGateway {
    constructor(private client: AxiosInstance, private logger: bunyan) {}

    public async getTemplateByName(name: string): Promise<string> {
        try {
            const response = await this.client.get(`/template?name=${name}`);
            
            const template = String(response.data);
            this.logger.debug(`Template ${name}: "${template}"`);

            return template;
        } catch (error) {
            this.logger.error(error);
            throw error;
        }
    }
}

