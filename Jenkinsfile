pipeline {
    agent any

    options {
        timestamps()
        timeout(time: 10, unit: 'MINUTES')
        buildDiscarder(logRotator(numToKeepStr: '10'))
        disableConcurrentBuilds()
    }

    environment {
        IMAGE_NAME     = 'dotnetapp'
        CONTAINER_NAME = 'dotnetapp'
        HOST_PORT      = '8081'
        APP_PORT       = '8080'
    }

    stages {
        stage('Build (.NET)') {
            agent {
                docker {
                    image 'mcr.microsoft.com/dotnet/sdk:8.0'
                    args '-e DOTNET_CLI_HOME=/tmp -e HOME=/tmp'
                }
            }
            steps {
                sh 'dotnet --version'
                sh 'dotnet restore'
                sh 'dotnet build -c Release --no-restore'
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
                sh 'sleep 5 && curl -f http://localhost:${HOST_PORT}/api/status'
            }
        }
    }

    post {
        success { echo 'Deployed successfully' }
        failure { echo 'Pipeline failed' }
    }
}
