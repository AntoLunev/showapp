pipeline 
{
    environment 
    {
        // This registry is important for removing the image after the tests
        registry = "antonlunev/showapp"
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
                    // Building the Docker image
                    def dockerImage = docker.build(registry + ":${env.BUILD_NUMBER}")
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
                    try 
                    {
                        echo "Running tests";
                        dockerImage.inside() 
                        {
                            echo "test inside container 1"
                            // Extracting the SOURCEDIR environment variable from inside the container
                            def SOURCEDIR = sh(script: 'echo \$SOURCEDIR', returnStdout: true).trim()

                            echo "test inside container 2"
                            // Running the tests inside the new directory
                            dir("$SOURCEDIR") 
                            {
                                sh "dotnet test ./showapp.tests/"
                            }
                        }
                    } 
                    catch (Exception ex)
                    {
                        echo "there is an error!"
                        testPassed = false

                        echo "Error is: ${ex}"
                    }
                    echo "was it successfull? ${testPassed}"
                }
            }
        }
        stage("Upload image to repo")
        {
            steps
            {
                script
                {
                    if (testPassed)
                    {
                        // Push generated docker image to the registry
                        dockerImage.push()
                        // Also push it with a "latest" tag
                        dockerImage.push('latest')
                    }
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