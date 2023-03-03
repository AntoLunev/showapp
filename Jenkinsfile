pipeline 
{
    environment 
    {
        // This registry is important for removing the image after the tests
        registry = "antonlunev/showapp"
        dockerhublogin = credentials('dockerhublogin')
    }
    agent any
    stages 
    {
        stage("Build image")
        {
            steps
            {
                script
                {
                    try
                    {
                        // Building the Docker image
                        dockerImage = docker.build(registry + ":${env.BUILD_NUMBER}")
                    }
                    catch (Exception ex)
                    {
                        echo "Error building image:"
                        echo "$ex"
                        error "Image build failed!"
                    }
                }
            }
        }
        stage("Test") 
        {
            steps 
            {
                script
                {
                    testPassed = true
                    catchError(buildResult: 'SUCCESS', stageResult: 'FAILURE') 
                    {
                        try
                        {
                            dockerImage.inside() 
                            {
                                // Extracting the SOURCEDIR environment variable from inside the container
                                def SOURCEDIR = sh(script: 'echo \$SOURCEDIR', returnStdout: true).trim()

                                // Running the tests
                                sh "dotnet test ${SOURCEDIR}/showapp.tests/"
                            }
                        } 
                        catch (Exception ex)
                        {
                            echo "Error testing image:"
                            echo "$ex"
                            testPassed = false

                            error "Test failed!"
                        }
                    }
                }
            }
        }
        stage("Upload image to repo")
        {
            when 
            { 
                // Do this step only when image and code inside were successfullyu tested
                equals expected: true, actual: testPassed 
            }
            steps
            {
                script
                {
                    // Authenticate to DockerHub
                    sh 'docker login -u $dockerhublogin_USR -p $dockerhublogin_PSW'
                    // Push generated docker image to the registry
                    dockerImage.push()
                    // Also push it with a "latest" tag
                    dockerImage.push('latest')
                    }
            }
        }
        stage("Cleanup")
        {
            steps
            {
                script
                {
                    // Removing the docker image
                    sh "docker rmi $registry:$BUILD_NUMBER"
                    sh "docker rmi $registry:latest"
                }

            }
        }
        stage("Report")
        {
            steps
            {
                script
                {
                    if (!testPassed)
                    {
                        error "Tests failed!"
                    }
                    // Send email with notification if test failed or passed
                }
            }
        }
    }
}