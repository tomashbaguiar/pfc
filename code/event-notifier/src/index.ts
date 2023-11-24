import dotenv from 'dotenv';
import bunyan, { LogLevel } from 'bunyan';

dotenv.config();

const appName: string = process.env.APP || 'event-notifier';
const logLevel: string = process.env.LOG_LEVEL || 'info';

const logger = bunyan.createLogger({
    name: appName,
    level: logLevel as LogLevel,
});

import { ZBClient } from 'zeebe-node';
import { connect } from 'mqtt';

const gatewayAddress: string = process.env.ZEEBE_GATEWAY || 'localhost:26500';
const zbc = new ZBClient(gatewayAddress, { stdout: logger });

let mqttClient = connect('mqtt://mqtt_broker');

zbc.createWorker({
    taskType: 'notify',
    taskHandler: async job => {
        try {
            logger.debug(job.variables);
            const data = job.variables.result;
            logger.debug(`Input data: '${data}'`);

            await mqttClient.publishAsync('spaceship/monitoring', data);

            return job.complete();
        } catch (error) {
            logger.error(error);
            return job.error('TRANSFORM_ERROR');
        }
    },
    onReady: () => logger.info('Transform worker connected!'),
    onConnectionError: () => logger.error('Transform worker disconnected!'),
});

