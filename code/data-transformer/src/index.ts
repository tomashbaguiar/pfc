import dotenv from 'dotenv';

import bunyan, { LogLevel } from 'bunyan';

dotenv.config();

const appName: string = process.env.APP || 'data-transformer';
const logLevel: string = process.env.LOG_LEVEL || 'info';

const logger = bunyan.createLogger({
    name: appName,
    level: logLevel as LogLevel,
});

import axios, { AxiosInstance } from 'axios';
import { ZBClient } from 'zeebe-node';
import { TemplateGateway } from './gateways/template.gateway';
import Handlebars from 'handlebars';

const templateUrl: string | undefined = process.env.TEMPLATE_URL;
const client: AxiosInstance = axios.create({
    baseURL: templateUrl,
    timeout: 1000,
});
const templateGateway = new TemplateGateway(client, logger);

const gatewayAddress: string = process.env.ZEEBE_GATEWAY || 'localhost:26500';
const zbc = new ZBClient(gatewayAddress, { stdout: logger });

zbc.createWorker({
    taskType: 'transform',
    taskHandler: async job => {
        try {
            logger.debug(job.variables);
            const templateName: string = job.variables.template;
            const data = job.variables.data;
            logger.debug(`Input data: '${data}'`);

            const templateString = await templateGateway.getTemplateByName(templateName);
            let template = Handlebars.compile(templateString);
            const result = template(data);
            logger.debug('Result' + data);

            return job.complete({ result });
        } catch (error) {
            logger.error(error);
            return job.error('TRANSFORM_ERROR');
        }
    },
    onReady: () => logger.info('Transform worker connected!'),
    onConnectionError: () => logger.error('Transform worker disconnected!'),
});