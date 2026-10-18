pipeline {
    agent any

    environment {
        IMAGE_NAME     = 'dotnetapp'
        CONTAINER_NAME = 'dotnetapp'
        HOST_PORT      = '8081'
        APP_PORT       = '8080'
    }

    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }

        stage('Build Docker Image') {
            steps {
                sh 'docker build -t ${IMAGE_NAME}:${BUILD_NUMBER} -t ${IMAGE_NAME}:latest .'
            }
        }

        stage('Deploy') {
            steps {
                sh '''
                    docker stop ${CONTAINER_NAME} || true
                    docker rm ${CONTAINER_NAME} || true
                    docker run -d \
                      --name ${CONTAINER_NAME} \
                      --restart unless-stopped \
                      -p ${HOST_PORT}:${APP_PORT} \
                      ${IMAGE_NAME}:latest
                '''
            }
        }

        stage('Health Check') {
            steps {
                sh '''
                    sleep 5
                    curl -f http://localhost:${HOST_PORT}/api/status
                '''
            }
        }
    }

    post {
        success { echo 'Deployed successfully on port 8081' }
        failure { echo 'Pipeline failed, check the console output' }
    }
}
